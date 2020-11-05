// Copyright (c) 2020 Terry Paton. All rights reserved.
// This work is licensed under the terms of the MIT license.  
// For a copy, see <https://opensource.org/licenses/MIT>.

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected PlayerCollider playerCollider;

    [SerializeField]
    protected Rigidbody rigidBodyRef;

    [Space(10)]
    [SerializeField]
    protected Transform rotator;

    [SerializeField]
    protected MeshRenderer deadBody;

    [Header("Managers")]
    [SerializeField]
    protected TrailManager trailManager;

    [SerializeField]
    protected PlayerModelManager playerModelManager;

    [Space(10)]
    [SerializeField]
    protected GameSettings gameSettings;

    private float health;
    private float timePassed;

    private Vector3 lastPosition;

    private float playerZ;
    private float currentThrust;
    private float currentZSpeed = 1.0f;

    private Vector3 playerRotation = Vector3.zero;

    private const float collisionPushBackForce = 1.0f;

    public float CurrentThrust
    {
        get => currentThrust;
    }

    public float TimePassed
    {
        get => timePassed;
        set => timePassed = value;
    }

    public void MoveToTargetPosition(Vector3 position)
    {
        position.y = 0.0f;

        rigidBodyRef.MovePosition(position);
    }

    public void Reset()
    {
        currentThrust = 0.0f;
        TimePassed = 0.0f;
        playerZ = 0.0f;
        currentZSpeed = 0.0f;
        if (gameSettings.easyMode)
        {
            health = GameSettings.EasyHealthReset;
        }
        else
        {
            health = GameSettings.HealthReset;
        }

        var resetPosition = new Vector3(6, 0, 0);

        StopPhysicsAndSetPosition(resetPosition);

        playerCollider.Reset();

        CoreConnector.UIControl.DisplayHealth();
        DisableDeadBody();

        ChangeShip(PlayerDisplayType.type0);

        playerCollider.EnableCollider();
        if (gameSettings.GodMode)
        {
            playerCollider.DisableCollider();
        }
    }

    protected void OnEnable()
    {
        CoreConnector.Player = this;
    }

    protected void OnDisable()
    {
        CoreConnector.Player = null;
    }

    private void DisableDeadBody()
    {
        deadBody.enabled = false;
    }

    private void EnableDeadBody()
    {
        deadBody.enabled = true;
        deadBody.transform.localEulerAngles = Random.insideUnitSphere * 180.0f;

        var randomForce = Random.onUnitSphere * 50.0f;
        randomForce.y = 0.0f;
        rigidBodyRef.AddRelativeTorque(randomForce);
    }

    public void MovePlayerToFinalPosition()
    {
        // move the player to the center of the screen
        CoreConnector.GameInput.SetCurrentPositionX( 6.0f);
    }

    public Vector3 GetCurrentPosition()
    {
        var position = rigidBodyRef.transform.position;
        return position;
    }

    public Vector3 GetCameraFocusPosition()
    {
        var position = rigidBodyRef.transform.position;
        position.z = playerZ;
        return position;
    }

    public bool HasPlayerReachedFinalStop()
    {
        var finalStop = CoreConnector.Levels.GetFinishLineStopPosition();

        var playerZ = GetCurrentPosition().z;

        if (playerZ > finalStop.z)
        {
            return true;
        }

        return false;
    }

    public void ManageMovement()
    {
        TimePassed += Time.deltaTime;
        playerZ += CurrentThrust;
    }

    private void CalculatePlayerSpeedBasedOnPercent()
    {
        var percent = TimePassed / gameSettings.totalLevelTime;
        percent = Mathf.Clamp01(percent);
        var sampledPercent = gameSettings.speedCurve.Evaluate(percent);
        sampledPercent = Mathf.Clamp01(sampledPercent);
        currentZSpeed = gameSettings.minVelocity +
                        sampledPercent * (gameSettings.maxVelocity - gameSettings.minVelocity);

        var diff = currentZSpeed - CurrentThrust;
        currentThrust += diff * 0.8f;
    }

    public void ManageGamePlay()
    {
        ManageMovement();

        ManageAndDisplayPlayerRotation();
        playerCollider.UpdateLoop();

        ManageHeight();

        CheckIfLevelCompleted();
    }

    public void ManageAndDisplayPlayerRotation()
    {
        // get the amoutn of rotation from Game
        var difference = Mathf.Abs(CoreConnector.GameInput.differenceInMovement.sqrMagnitude);
        if (difference > 0.0f)
        {
            var rotationAmount = CoreConnector.GameInput.differenceInMovement.x * gameSettings.PlayerRotationRange;
            rotationAmount = Mathf.Clamp(rotationAmount, -160.0f, 160.0f);
            if (rotationAmount > 0 && rotationAmount > playerRotation.z ||
                rotationAmount < 0 && rotationAmount < playerRotation.z)
            {
                playerRotation.z = rotationAmount;
            }
        }

        playerRotation.z *= 0.9f;
        rotator.localEulerAngles = playerRotation;
    }

    private void CheckIfLevelCompleted()
    {
        var playerPosition = GetCurrentPosition();
        var levelCompletedPoint = CoreConnector.Levels.GetLevelEndLocation();

        var levelEndZ = levelCompletedPoint.z;

        if (playerPosition.z > levelEndZ)
        {
            CoreConnector.CoreGameControl.LevelCompleted();
        }
    }

    private void ManageHeight()
    {
        // make sure the player is at the correct height
        var position = GetCurrentPosition();
        var targetPosition = position;
        if (Math.Abs(position.y - targetPosition.y) > 0.01f)
        {
            targetPosition.y = gameSettings.playerHeight;
            rigidBodyRef.MovePosition(targetPosition);
        }
    }

    public void Trigger(Collider colliderRef)
    {
        var layerName = LayerMask.LayerToName(colliderRef.gameObject.layer);
        if (layerName == ObstacleLayers.Coins)
        {
            CollectCoin(colliderRef.transform.position);
        }
    }

    private void AddHealth(float amount)
    {
        if (gameSettings.easyMode)
        {
            amount *= gameSettings.easyHealthModifier;
        }

        health += amount;
        health = Mathf.Clamp01(health);
        CoreConnector.UIControl.DisplayHealth();
    }

    public void Collision(ContactPoint contact, bool collisionIsActive)
    {
        var layerName = LayerMask.LayerToName(contact.otherCollider.gameObject.layer);
        if (layerName == ObstacleLayers.Coins)
        {
            CollectCoin(contact.point);
        }

        // don't detect any collisions because theres one happening
        if (collisionIsActive)
        {
#if UNITY_EDITOR
            Debug.Log("Collision active already");
#endif
            return;
        }

        switch (layerName)
        {
            case ObstacleLayers.Obstacles:
                ObstacleCollision(contact);
                break;
            case ObstacleLayers.SideWalls:
                SideWallsCollision(contact);
                break;
            default:
#if UNITY_EDITOR
                Debug.Log("Collision not handled");
#endif
                break;
        }
    }

    private void SideWallsCollision(ContactPoint contact)
    {
        DamagePlayer(gameSettings.sideWallsDamage);
        PushPlayerBasedOnSideOfScreen();
        CoreConnector.ParticleManager.ShowCollision(contact.point);
    }

    private void PushPlayerBasedOnSideOfScreen()
    {
        var playerPosition = GetCurrentPosition();
        if (playerPosition.x > 0)
        {
            CoreConnector.GameInput.PushBackPlayer(collisionPushBackForce);
        }
        else
        {
            CoreConnector.GameInput.PushBackPlayer(-collisionPushBackForce);
        }
    }

    private void ObstacleCollision(ContactPoint contact)
    {
        DamagePlayer(gameSettings.obstacleDamage);
        PushPlayerBasedOnSideOfScreen();
        CoreConnector.ParticleManager.ShowDestructableCollision(contact.point);
    }

    private void CollectCoin(Vector3 effectPosition)
    {
        AddHealth(gameSettings.healthPickup);
        CoreConnector.ParticleManager.ShowCollect(effectPosition);
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.CollectLife);
    }

    private void DamagePlayer(float damageAmount)
    {
        if (gameSettings.easyMode)
        {
            damageAmount *= gameSettings.easyDamageModifier;
        }

        PlayRandomCollisionSound();
        health -= damageAmount;
        if (health <= 0.0f)
        {
            ExplodePlayer();
            // Player has been killed
            CoreConnector.CoreGameControl.PlayerKilled();
        }

        CoreConnector.UIControl.DisplayHealth();
    }

    private void PlayRandomCollisionSound()
    {
        CoreConnector.SoundManager.PlaySound( SoundManager.Sounds.Collision);
    }

    private void ExplodePlayer()
    {
        playerModelManager.DisableAllModels();
        var position = GetCurrentPosition();
        CoreConnector.ParticleManager.ShowExplosion(position);
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.Explosion);

        EnableDeadBody();

        // stop thrust
        trailManager.DisableAllTrails();
    }

    private void StopPhysicsAndSetPosition(Vector3 position)
    {
        rigidBodyRef.position = position;
        rigidBodyRef.transform.position = position;
        rigidBodyRef.angularVelocity = Vector3.zero;
        rigidBodyRef.velocity = Vector3.zero;
    }

    public void ChangeShip(PlayerDisplayType displayType)
    {
        var position = GetCurrentPosition();
        position.z += 2.0f;
        CoreConnector.ParticleManager.ShowChangeShip(position);

        trailManager.Show(displayType);
        playerModelManager.Show(displayType);

        AddHealth(0.1f);
        // each ship has a specific set power based on how far through the level you are
        CalculatePlayerSpeedBasedOnPercent();

        CoreConnector.CameraControl.PunchBackSun();
        // Don't playa sound for the first ship
        if (displayType != PlayerDisplayType.type0)
        {
            CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.ChangeShip);
        }
    }

    public void StopTrails()
    {
        trailManager.DisableAllTrails();
    }

    public float GetCurrentHealth()
    {
        return health;
    }
}