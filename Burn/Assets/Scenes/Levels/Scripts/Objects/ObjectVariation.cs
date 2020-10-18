using UnityEngine;

public class ObjectVariation : MonoBehaviour
{
    [SerializeField]
    protected MeshRenderer[] meshRenderers;

    public virtual void Show()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }

    public virtual  void Hide()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }
}