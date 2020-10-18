using UnityEngine;

public class EdgeAsteroid : NonCollisionObjectBase
{
    public override void Init()
    {
        RandomlyScale();
        RandomlyRotate();
    }

    protected override void RandomlyScale()
    {
        transform.localScale = Vector3.one * Random.Range(0.7f, 1.2f);
    }

    protected override void RandomlyRotate()
    {
        rotationAmount = Random.insideUnitSphere * 180f;
        visualObject.localEulerAngles = rotationAmount;
    }
}