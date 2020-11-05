#region

using UnityEngine;

#endregion

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    protected AudioSource gameMusicSource;

    [SerializeField]
    protected AudioSource levelCompleteSource;

    [SerializeField]
    protected AudioSource transitionSoundSource;

    [SerializeField]
    protected AudioSource startLevelSource;

    [SerializeField]
    protected AudioSource thanksSource;

    [SerializeField]
    protected AudioSource buttonPressSource;

    [SerializeField]
    protected AudioSource playerAudioSource;

    [Space(5)]
    [SerializeField]
    protected AudioClip explosionClip;

    [SerializeField]
    protected AudioClip alertClip;

    [SerializeField]
    protected AudioClip collisionClip;

    [SerializeField]
    protected AudioClip collectLifeClip;

    [SerializeField]
    protected AudioClip changeShipClip;

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
                playerAudioSource.PlayOneShot(alertClip);
                break;
            case Sounds.ButtonPress:
                buttonPressSource.Play();
                break;
            case Sounds.Explosion:
                playerAudioSource.PlayOneShot(explosionClip);
                break;
            case Sounds.Collision:
                playerAudioSource.PlayOneShot(collisionClip);
                break;
            case Sounds.CollectLife:
                playerAudioSource.PlayOneShot(collectLifeClip);
                break;
            case Sounds.GameMusic:
                gameMusicSource.Stop();
                gameMusicSource.Play();
                break;
            case Sounds.ChangeShip:
                playerAudioSource.PlayOneShot(changeShipClip);
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
            case Sounds.ButtonPress:
                buttonPressSource.Stop();
                break;
            case Sounds.Explosion:
                playerAudioSource.Stop();
                break;
            case Sounds.Collision:
                playerAudioSource.Stop();
                break;
            case Sounds.CollectLife:
                playerAudioSource.Stop();
                break;
            case Sounds.GameMusic:
                gameMusicSource.Stop();
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