using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField]
    protected Transform sunRotator;

    [SerializeField]
    protected Transform sunHolder;

    [Space(10), SerializeField]
    protected ParticleSystem sunParticles;

    [Space(10), SerializeField]
    protected Renderer[] sunRenderers;

    private Vector3 rotation;
    private Vector3 sunHolderLocation;

    private float sunPunchedBackOffset;
    private bool punchBackQueued;
    private const float SunPunchAmount = -10.0f;

    protected void Update()
    {
        if (!CoreConnector.CoreGameControl.IsGameRunning)
        {
            return;
        }

        rotation.z += Time.deltaTime * 2.0f;
        rotation.z %= 360.0f;
        sunRotator.localEulerAngles = rotation;

        ManageSunPunchBack();
    }

    private void ManageSunPunchBack()
    {
        if (punchBackQueued)
        {
            sunPunchedBackOffset -= 0.3f;
            if (sunPunchedBackOffset < SunPunchAmount)
            {
                sunPunchedBackOffset = SunPunchAmount;
                punchBackQueued = false;
            }
        }
        else
        {
            if (sunPunchedBackOffset < 0.0f)
            {
                sunPunchedBackOffset *= 0.998f;
                sunPunchedBackOffset = Mathf.Clamp(sunPunchedBackOffset, SunPunchAmount, 0.0f);
            }
        }

        sunHolderLocation.z = sunPunchedBackOffset;
        sunHolder.localPosition = sunHolderLocation;
    }

    protected void OnEnable()
    {
        rotation.z = Random.Range(0.0f, 360.0f);
        DisableRenderers();
    }

    public void PunchSunBack()
    {
        punchBackQueued = true;
    }

    public void DisableRenderers()
    {
        foreach (var sunRenderer in sunRenderers)
        {
            sunRenderer.enabled = false;
        }

        sunParticles.Stop();
    }

    public void EnableRenderers()
    {
        foreach (var sunRenderer in sunRenderers)
        {
            sunRenderer.enabled = true;
        }

        sunParticles.Play();
    }
}