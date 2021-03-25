#region

using System.Collections.Generic;
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

    private Dictionary<Sounds, AudioSource> sounds;

    protected void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        CoreConnector.SoundManager = this;
        sounds = new Dictionary<Sounds, AudioSource>
        {
            {Sounds.Alert, playerAudioSource},
            {Sounds.ButtonPress, buttonPressSource},
            {Sounds.LevelComplete, levelCompleteSource},
            {Sounds.TransitionSound, transitionSoundSource},
            {Sounds.StartLevel, startLevelSource},
            {Sounds.Thanks, thanksSource}
        };
    }

    protected void OnDisable()
    {
        CoreConnector.SoundManager = null;
    }

    public void PlaySound(Sounds soundID)
    {
        // handle some special cases
        switch (soundID)
        {
            case Sounds.Explosion:
                playerAudioSource.PlayOneShot(explosionClip);
                return;
            case Sounds.Collision:
                playerAudioSource.PlayOneShot(collisionClip);
                return;
            case Sounds.CollectLife:
                playerAudioSource.PlayOneShot(collectLifeClip);
                return;
            case Sounds.GameMusic:
                gameMusicSource.Stop();
                gameMusicSource.Play();
                return;
            case Sounds.ChangeShip:
                playerAudioSource.PlayOneShot(changeShipClip);
                return;
        }

        var soundSource = GetSoundSource(soundID);
        soundSource.Play();
    }

    public void StopSound(Sounds soundID)
    {
        switch (soundID)
        {
            case Sounds.CollectLife:
                playerAudioSource.Stop();
                break;
            case Sounds.GameMusic:
                gameMusicSource.Stop();
                break;
            case Sounds.LevelComplete:
                levelCompleteSource.Stop();
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

    private AudioSource GetSoundSource(Sounds soundID)
    {
        sounds.TryGetValue(soundID, out var soundSource);
        return soundSource;
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
        Thanks
    }
}