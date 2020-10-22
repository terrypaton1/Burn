using UnityEngine;

public class Asteroid : RespawnableObject
{
    [SerializeField]
    protected Transform visualObject;

    private Vector3 rotationAmount;
    private MeshRenderer currentRenderer;

    public override void Reset()
    {
        base.Reset();
        RandomizeScale();
        RandomizeRotation();
        RandomizeAsteroidMesh();
    }

    public override void SetCollidedWith()
    {
        base.SetCollidedWith();
        CoreConnector.ParticleManager.ShowAsteroidExplosion(visualObject.position);
        DisableVisuals();
    }

    public override void UpdateLoop()
    {
        visualObject.Rotate(rotationAmount);
    }

    private void RandomizeScale()
    {
        visualObject.localScale = Vector3.one * Random.Range(0.7f, 1.1f);
    }

    private void RandomizeRotation()
    {
        rotationAmount = Random.insideUnitSphere * 0.5f;
        visualObject.localEulerAngles = rotationAmount;
    }

    private void RandomizeAsteroidMesh()
    {
        DisableVisuals();
        currentShowingIndex = Random.Range(0, objectVariations.Length);
    }
}