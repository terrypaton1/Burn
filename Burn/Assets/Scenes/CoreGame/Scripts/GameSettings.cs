using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public static bool DrawWallSidesDebug = false;

    public static readonly float HealthReset = 0.5f;
    public static readonly float EasyHealthReset = 0.75f;
    public static readonly float HealthMax = 1.0f;

    public static readonly float CellLength = 100.0f;
    public static readonly float CellWidth = 12.0f;
    public static readonly float SolarFlareZOffset = -15.0f;

    [Header("Movement")]
    [Range(0, 3)]
    [SerializeField]
    public float playerHeight = 10.0f;

    [Range(0.0f, 10.0f)]
    [SerializeField]
    public float maxVelocity = 6;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float minVelocity = 5.0f;

    [SerializeField]
    [Range(10.0f, 180.0f)]
    public float PlayerRotationRange = 30.0f;

    [SerializeField]
    [Range(0.1f, 1.0f)]
    public float horizontalMovementFactor = 0.35f;

    [SerializeField]
    public float xMovementRange = 28.0f;

    [SerializeField]
    public float playerMaxMovementDelta;

    [Range(0.0f, 90.0f)]
    [SerializeField]
    public float deviceYTiltValue = 5.0f;

    [Range(0.0f, 90.0f)]
    [SerializeField]
    public float deviceTiltLerp = 0.1f;

    [Header("Misc")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float healthPickup = 0.1f;

    [SerializeField]
    public float totalLevelTime = 3000.0f;

    [Header("Damage")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float obstacleDamage = 0.035f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float sideWallsDamage = 0.015f;

    [Header("Movement")]
    [SerializeField]
    public AnimationCurve speedCurve;

    [SerializeField]
    public AnimationCurve uiTweenInCurve;

    [SerializeField]
    public AnimationCurve uiTweenOutCurve;

    [Header("Extra")]
    [SerializeField]
    public bool GodMode;

    [SerializeField]
    public bool easyMode;

    [SerializeField]
    public float easyDamageModifier = 0.7f;

    [SerializeField]
    public float easyHealthModifier = 1.25f;
}