using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField]
    private Renderer[] modelMeshRenderers;

    protected void OnEnable()
    {
        if (!Application.isPlaying)
        {
            modelMeshRenderers = GetComponentsInChildren<Renderer>();
        }
    }

    public virtual void Show()
    {
        foreach (var mesh in modelMeshRenderers)
        {
            mesh.enabled = true;
        }
    }

    public virtual void Hide()
    {
        foreach (var mesh in modelMeshRenderers)
        {
            mesh.enabled = false;
        }

        transform.localPosition = Vector3.zero;
    }
}