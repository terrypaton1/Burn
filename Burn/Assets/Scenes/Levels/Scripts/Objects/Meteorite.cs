using System.Collections;
using UnityEngine;

public class Meteorite : RespawnableObject
{
    [SerializeField]
    protected Transform visualObject;

    [SerializeField]
    protected Rigidbody rigidBodyRef;

    private Vector3 rotationAmount;

    public override void Reset()
    {
        base.Reset();
        RandomlyScale();
        RandomlyRotate();

        rigidBodyRef.isKinematic = true;
        rigidBodyRef.Sleep();
    }

    private void RandomlyScale()
    {
        visualObject.localScale = Vector3.one * Random.Range(0.7f, 1.5f);
    }

    private void RandomlyRotate()
    {
        rotationAmount = Random.insideUnitSphere * 0.5f;
        visualObject.localEulerAngles = rotationAmount;
    }

    public override void UpdateLoop()
    {
        visualObject.Rotate(rotationAmount);
        StopIfOutOfWorldLimits();
    }

    private void StopIfOutOfWorldLimits()
    {
        var xpos = transform.position.x;
        if (xpos < -50.0f || xpos > 50.0f)
        {
            // gone outside the world limits
            Stop();
        }
    }

    private void Stop()
    {
        rigidBodyRef.isKinematic = true;
        colliderRef.enabled = false;
    }

    public override void SetCollidedWith()
    {
        base.SetCollidedWith();

        CoreConnector.ParticleManager.ShowDestructableCollision(transform.position);
    }

    public void Launch(Vector3 force)
    {
        StopRunningCoroutine();

        coroutine = LaunchSequence(force);
        StartCoroutine(coroutine);
    }

    private IEnumerator LaunchSequence(Vector3 force)
    {
        EnableVisuals();
        rigidBodyRef.isKinematic = false;
        colliderRef.enabled = true;
        rigidBodyRef.WakeUp();

        yield return null;

        rigidBodyRef.velocity = force;
        rigidBodyRef.angularVelocity = Random.insideUnitSphere * 180f;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        rigidBodyRef.angularVelocity = Vector3.zero;
        rigidBodyRef.velocity = Vector3.zero;
    }

    public override void EnableVisuals()
    {
        base.EnableVisuals();
    }

    public override void DisableVisuals()
    {
        base.DisableVisuals();
        Stop();
    }
}