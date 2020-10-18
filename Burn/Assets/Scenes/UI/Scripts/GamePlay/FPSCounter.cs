using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    protected Text text;

    private float timer;
    private float refresh = 1.0f;
    private float averageFramerate;
    private readonly string display = "FPS:{0}";
    private float updateTime;

    protected void OnEnable()
    {
        text.text = "";
    }

    private void Update()
    {
        CalculateAndDisplayFPS();
    }

    private void CalculateNextTime()
    {
        updateTime = Time.time + 1.0f;
    }

    private void CalculateAndDisplayFPS()
    {
        var timelapse = Time.smoothDeltaTime;
        timer = ManageTime(timelapse);

        if (timer <= 0.0f)
        {
            averageFramerate = (int) (1.0f / timelapse);
        }
        
        // only update the text field every 1 second.
        if (Time.time < updateTime)
        {
            return;
        }
        CalculateNextTime();
        
        text.text = string.Format(display, averageFramerate.ToString(CultureInfo.InvariantCulture));
    }

    private float ManageTime(float timelapse)
    {
        if (timer <= 0)
            return refresh;
        else
            return timer -= timelapse;
    }
}