using UnityEngine;

public class RotateThanksModels : MonoBehaviour
{
    [SerializeField]
    public int speed = 1;

    protected void OnEnable()
    {
        var rotation = Vector3.up * Random.Range(-180.0f, 180.0f);
        transform.Rotate(rotation);
    }

    public void Update()
    {
        var rotation = Vector3.up * (Time.deltaTime * speed);
        transform.Rotate(rotation);
    }
}