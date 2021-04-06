using UnityEngine;
using UnityEngine.UI;

public class UIGroup : MonoBehaviour
{
    [SerializeField]
    protected Renderer[] renderers;

    [SerializeField]
    protected Image[] images;

    [SerializeField]
    protected Text[] text;

    protected bool showing;
    public int showCount;

    public int ShowCount
    {
        get { return showCount; }
    }

    protected void OnEnable()
    {
        HideInstantly();
    }

    public virtual void Show()
    {
        showing = true;
        showCount = ShowCount + 1;
        // todo make this an animated process
        SetRendererState(true);
    }

    public virtual void Hide()
    {
        if (!showing)
        {
            return;
        }

        showing = false;
        SetRendererState(false);
    }

    public virtual void HideInstantly()
    {
        showing = false;
        SetRendererState(false);
    }

    protected virtual void SetRendererState(bool state)
    {
        foreach (var rendererRef in renderers)
        {
            rendererRef.enabled = state;
        }

        foreach (var imageRef in images)
        {
            imageRef.enabled = state;
        }

        foreach (var textRef in text)
        {
            textRef.enabled = state;
        }
    }
}