using UnityEngine;

public class MainMenuSun : MonoBehaviour
{
    [SerializeField]
    protected Transform sunTransform;

    [SerializeField]
    protected Transform sunGlow1;

    private const float maxTime = 25.0f;
    private Vector3 scale;
    private readonly Vector3 scaleRange = new Vector3(0.5f, 0.5f, 0.5f);
    private float timePassed;

    protected void Update()
    {
        var percent = CalculatePercent();
        SetScale(percent);

        sunGlow1.Rotate(Vector3.forward, 0.0001f);
    }

    private float CalculatePercent()
    {
        timePassed += Time.deltaTime;
        var percent = timePassed / maxTime;
        if (percent > 1.0f)
        {
            timePassed = 0.0f;
        }

        return percent;
    }

    protected void OnEnable()
    {
        timePassed = 0.0f;
        SetScale(0.0f);
    }

    private void SetScale(float percent)
    {
        scale = Vector3.one + scaleRange * percent;
        var scaleLerp = Vector3.Lerp(sunTransform.localScale, scale, Time.deltaTime);
        sunTransform.localScale = scaleLerp;
    }
}