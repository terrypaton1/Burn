using UnityEngine;

public class DistantObject : NonCollisionObjectBase
{
    private const float y1 = -45;
    private const float y2 = -68;

    [SerializeField]
    protected Renderer rendererRef;

    [SerializeField]
    protected Material[] mats;

    public override void Init()
    {
        RandomlyScale();
        RandomlyRotate();
        SetMaterialOnY();
    }

    private void RandomlyScale()
    {
        transform.localScale = Vector3.one * Random.Range(0.7f, 2.2f);
    }

    private void SetMaterialOnY()
    {
        var yVal = Mathf.RoundToInt(transform.position.y);
        if (yVal > y1)
        {
            SetMaterials(mats[0]);
            return;
        }

        if (yVal > y2)
        {
            SetMaterials(mats[1]);
            return;
        }

        SetMaterials(mats[2]);
    }

    private void SetMaterials(Material newMaterial)
    {
        var materialsRef = rendererRef.materials;
        materialsRef[0] = newMaterial;
        rendererRef.materials = materialsRef;
    }

    public override void DisableRenderers()
    {
        rendererRef.enabled = false;
    }

    public override void EnableRenderers()
    {
        rendererRef.enabled = true;
    }
}