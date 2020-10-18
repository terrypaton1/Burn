using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem particle;

    public virtual void Show(Vector3 position)
    {
        transform.position = position;
        particle.Stop();
        particle.Play(true);
    }
}