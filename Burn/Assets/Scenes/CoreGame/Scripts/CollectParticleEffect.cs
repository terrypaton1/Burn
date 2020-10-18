using UnityEngine;

public class CollectParticleEffect : ParticleEffect
{
    [SerializeField]
    protected int emitQuantity = 50;

    public override void Show(Vector3 position)
    {
        transform.position = position;
        particle.Play();
        particle.Emit(emitQuantity);
    }
}