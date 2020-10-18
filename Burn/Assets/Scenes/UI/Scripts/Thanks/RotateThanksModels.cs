using UnityEngine;

public class RotateThanksModels : MonoBehaviour
{
    [SerializeField]
    public int speed = 1;

    public void Update()
    {
        var rotation = Vector3.up * (Time.deltaTime * speed);
        transform.Rotate(rotation);
    }
}