using UnityEngine;

public class DestructibleCollisionParticleEffect : ParticleEffect
{
    [SerializeField]
    protected int emitQuantity=50;

    public override void Show(Vector3 position)
    {
        transform.position = position;
        particle.Play(true);
        particle.Emit(emitQuantity);
    }
}