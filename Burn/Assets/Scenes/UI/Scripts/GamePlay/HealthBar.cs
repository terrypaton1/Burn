using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    protected Transform healthBarScaler;

    [SerializeField]
    protected Image barImage;

    [SerializeField]
    protected HealthWarning healthWarning;

    private Vector3 scale = new Vector3(0.01f, 1f, 1f);

    [SerializeField]
    private GameObject content;

    public void SetValue(float percent)
    {
        scale.x = percent;
        healthBarScaler.localScale = scale;

        var color = GetColourBasedOnPercent(percent);
        barImage.color = color;

        healthWarning.Display(percent);
    }

    private static Color GetColourBasedOnPercent(float percent)
    {
        if (percent < 0.25f)
        {
            return Color.red;
        }

        if (!(percent < 0.55f))
        {
            return Color.green;
        }

        var orange = Color.yellow + Color.red * 0.25f;
        return orange;
    }

    public void HideInstantly()
    {
        content.SetActive(false);
    }

    public void Show()
    {
        content.SetActive(true);
        healthWarning.Display(1.0f);
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}