using UnityEngine;

public class EndCell : Cell
{
    [SerializeField]
    protected FinalStop finalStop;

    [SerializeField]
    private ParticleSystem[] particles;

    public override void Reset()
    {
        base.Reset();
        StopParticles();
    }

    public Vector3 GetFinishLineStopPosition()
    {
        return finalStop.transform.position;
    }

    public Vector3 GetLevelEndLocation()
    {
        return transform.position;
    }

    public void StartLevelCompleteEffects()
    {
        CoreConnector.SoundManager.PlaySound(SoundManager.Sounds.LevelComplete);
        foreach (var particle in particles)
        {
            particle.Play();
        }
    }

    public override void DisableRenderers()
    {
        base.DisableRenderers();
        StopParticles();
    }

    private void StopParticles()
    {
        foreach (var particle in particles)
        {
            particle.Stop();
            particle.Clear();
        }
    }
}