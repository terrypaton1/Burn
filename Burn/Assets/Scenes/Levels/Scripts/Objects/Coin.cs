using UnityEngine;

public class Coin : RespawnableObject
{
    [SerializeField]
    protected Transform visualObject;

    private Vector3 rotationAmount;

    public override void Reset()
    {
        base.Reset();
        rotationAmount = new Vector3(0, 0.1f, 0);
        RandomlyRotateObject();

        DisableVisuals();
    }

    private void RandomlyRotateObject()
    {
        var random = Random.Range(0.0f, 360.0f);
        var randomStart = new Vector3(0.0f, random, 0.0f);
        visualObject.localEulerAngles = randomStart;
    }

    public override void UpdateLoop()
    {
        visualObject.Rotate(rotationAmount);
    }

    public override void SetCollidedWith()
    {
        base.SetCollidedWith();
        DisableVisuals();
    }
}