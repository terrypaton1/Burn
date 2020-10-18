using UnityEngine;

public class CoinObjectVariation : ObjectVariation
{
    [SerializeField]
    protected ParticleSystem particle;

    public override void Show()
    {
        base.Show();
        particle.Play();
    }

    public override void Hide()
    {
        base.Hide();
        particle.Stop();
    }
}