using UnityEngine;

public class NonCollisionObjectBase : MonoBehaviour
{
    [SerializeField]
    protected Transform visualObject;
    protected Vector3 rotationAmount;

    public virtual void Init()
    {
        transform.localScale = Vector3.one * Random.Range(0.7f, 1.2f);

        rotationAmount = Random.insideUnitSphere * 180f;
        visualObject.localEulerAngles = rotationAmount;
    }

    protected virtual void RandomlyScale()
    {
        transform.localScale = Vector3.one * Random.Range(0.7f, 1.2f);
    }

    protected virtual void RandomlyRotate()
    {
        rotationAmount = Random.insideUnitSphere * 180f;
        visualObject.localEulerAngles = rotationAmount;
    }

    public virtual void DisableRenderers()
    {
     
    }

    public virtual void EnableRenderers()
    {
      
    }
}