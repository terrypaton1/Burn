using UnityEngine;

public class SideOfScreenAsteroid : MonoBehaviour
{
    private Vector3 rotationAmount;

    protected void Update()
    {
        transform.Rotate(rotationAmount);
    }

    protected void OnEnable()
    {
        RandomizeScale();
        RandomizeRotation();
    }

    private void RandomizeScale()
    {
        transform.localScale = Vector3.one * Random.Range(0.7f, 1.1f);
    }

    private void RandomizeRotation()
    {
        rotationAmount = Random.insideUnitSphere * 0.5f;
        transform.localEulerAngles = rotationAmount;
    }
}