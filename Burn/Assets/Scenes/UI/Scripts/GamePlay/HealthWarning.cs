using UnityEngine;
using UnityEngine.UI;

public class HealthWarning : MonoBehaviour
{
    [SerializeField]
    private Image topBar;

    [SerializeField]
    private Image bottomBar;

    private void Show(float intensityPercent)
    {
        intensityPercent = Mathf.Clamp01(intensityPercent);

        var offset = Vector3.up;
        offset += (Vector3.down * intensityPercent);
        offset *= 100.0f;

        topBar.transform.localPosition = offset;
        bottomBar.transform.localPosition = -offset;

        topBar.enabled = true;
        bottomBar.enabled = true;
    }

    private void Hide()
    {
        topBar.enabled = false;
        bottomBar.enabled = false;
    }

    public void Display(float percent)
    {
        if (percent < 0.2f)
        {
            var displayAmount = 1 - (percent / 0.2f);
            Show(displayAmount);
        }
        else
        {
            Hide();
        }
    }
}