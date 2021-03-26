using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    [SerializeField]
    protected TrailRenderer[] trails;

    [SerializeField]
    protected ParticleSystem[] particles;

    public void Show()
    {
        foreach (var trail in trails)
        {
            trail.enabled = true;
            trail.emitting = true;
        }

        foreach (var particle in particles)
        {
            particle.Play();
        }
    }

    public void Hide()
    {
        foreach (var trail in trails)
        {
            trail.enabled = false;
            trail.emitting = false;
            trail.Clear();
        }

        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }
}