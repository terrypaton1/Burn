using UnityEngine;

public class UIGroup : MonoBehaviour
{
    [SerializeField]
    protected GameObject content;

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
        content.SetActive(true);
    }

    public virtual void Hide()
    {
        if (!showing)
        {
            return;
        }

        showing = false;
        content.SetActive(false);
    }

    public virtual void HideInstantly()
    {
        showing = false;
        content.SetActive(false);
    }
}