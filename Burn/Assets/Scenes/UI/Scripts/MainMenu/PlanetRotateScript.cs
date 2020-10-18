using UnityEngine;

public class PlanetRotateScript : MonoBehaviour
{
    public int speed = 1;

    public void Update()
    {
        transform.Rotate(Vector3.up * (Time.deltaTime * speed));
    }
}