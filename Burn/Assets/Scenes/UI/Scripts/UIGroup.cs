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
        // todo make the renderers get enabled/activated rather than the whole game object tree enabling etc
        //  content.SetActive(true);
        SetRendererState(true);
    }

    public virtual void Hide()
    {
        if (!showing)
        {
            return;
        }

        showing = false;
        // content.SetActive(false);
        SetRendererState(false);
    }

    public virtual void HideInstantly()
    {
        showing = false;
        //  content.SetActive(false);
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