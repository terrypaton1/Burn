using UnityEngine;

public class RotateUFO : MonoBehaviour
{
    [SerializeField]
    public int speed = 1;

    public void Update()
    {
        var rotation = Vector3.forward * (Time.deltaTime * speed);
        transform.Rotate(rotation);
    }
}