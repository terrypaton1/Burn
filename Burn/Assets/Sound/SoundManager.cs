#region

using UnityEngine;

#endregion

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    protected AudioSource buttonPressSource;

    [SerializeField]
    protected AudioSource alertSoundSource;

    [SerializeField]
    protected AudioSource explosionSource;

    [SerializeField]
    protected AudioSource collisionSource;

    [SerializeField]
    protected AudioSource solarFlareSource;

    [SerializeField]
    protected AudioSource collectLifeSource;

    [SerializeField]
    protected AudioSource collision2Source;

    [SerializeField]
    protected AudioSource collision3Source;

    [SerializeField]
    protected AudioSource gameMusicSource;

    [SerializeField]
    protected AudioSource changeShipSource;

    [SerializeField]
    protected AudioSource levelCompleteSource;

    [SerializeField]
    protected AudioSource transitionSoundSource;

    [SerializeField]
    protected AudioSource startLevelSource;
    
    [SerializeField]
    protected AudioSource thanksSource;
    
    protected void OnEnable()
    {
        CoreConnector.SoundManager = this;
    }

    protected void OnDisable()
    {
        CoreConnector.SoundManager = null;
    }

    public void PlaySound(Sounds soundID)
    {
        switch (soundID)
        {
            case Sounds.Alert:
                alertSoundSource.Play();
                break;
            case Sounds.ButtonPress:
                buttonPressSource.Play();
                break;
            case Sounds.Explosion:
                explosionSource.Play();
                break;
            case Sounds.Collision:
                collisionSource.Play();
                break;
            case Sounds.SolarFlare:
                solarFlareSource.Play();
                break;
            case Sounds.CollectLife:
                collectLifeSource.Play();
                break;

            case Sounds.Collision2:
                collision2Source.Play();
                break;

            case Sounds.Collision3:
                collision3Source.Play();
                break;
            case Sounds.GameMusic:
                gameMusicSource.Stop();
                gameMusicSource.Play();
                break;
            case Sounds.ChangeShip:
                changeShipSource.Play();
                break;
            case Sounds.LevelComplete:
                levelCompleteSource.Play();
                break;
            case Sounds.TransitionSound:
                transitionSoundSource.Play();
                break;
            case Sounds.StartLevel:
                startLevelSource.Play();
                break;
            case Sounds.Thanks:
                thanksSource.Play();
                break;
            default:
                Debug.Log("Sound ID not handled:" + soundID);
                break;
        }
    }
    public void StopSound(Sounds soundID)
    {
        switch (soundID)
        {
            case Sounds.Alert:
                alertSoundSource.Stop();
                break;
            case Sounds.ButtonPress:
                buttonPressSource.Stop();
                break;
            case Sounds.Explosion:
                explosionSource.Stop();
                break;
            case Sounds.Collision:
                collisionSource.Stop();
                break;
            case Sounds.SolarFlare:
                solarFlareSource.Stop();
                break;
            case Sounds.CollectLife:
                collectLifeSource.Stop();
                break;

            case Sounds.Collision2:
                collision2Source.Stop();
                break;

            case Sounds.Collision3:
                collision3Source.Stop();
                break;
            case Sounds.GameMusic:
                gameMusicSource.Stop();
                break;
            case Sounds.ChangeShip:
                changeShipSource.Stop();
                break;
            case Sounds.LevelComplete:
                levelCompleteSource.Stop();
                break;
            case Sounds.TransitionSound:
                transitionSoundSource.Stop();
                break;
            case Sounds.StartLevel:
                startLevelSource.Stop();
                break;
            case Sounds.Thanks:
                thanksSource.Stop();
                break;
            default:
                Debug.Log("Sound ID not handled:" + soundID);
                break;
        }
    }
    public enum Sounds
    {
        Explosion,
        ButtonPress,
        Collision,
        SolarFlare,
        Alert,
        CollectLife,
        StartLevel,
        TransitionSound,
        LevelComplete,
        ChangeShip,
        GameMusic,
        Collision2,
        Collision3,
        Thanks
    }
}