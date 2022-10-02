using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController soundController;
    public static float Volume
    {
        get
        {
            return soundController.volume;
        }
        set
        {
            soundController.volume = value;
            for (int i = 0; i < soundController.audioSources.Count; i++)
            {
                soundController.audioSources[i].volume = value;
            }
        }
    }
    [SerializeField]
    [Range(0, 1)]
    private float volume = 1;
    private List<AudioSource> audioSources = new List<AudioSource>();
    private AudioSource fixedPitchSource;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
        }
        fixedPitchSource = gameObject.AddComponent<AudioSource>();
        soundController = this;
        Volume = volume;
    }

    public static void PlaySound(AudioClip audioClip, bool stop = false)
    {
        if (stop)
        {
            soundController.fixedPitchSource.Stop();
        }
        if (audioClip == null)
        {
            return;
        }
        soundController.fixedPitchSource.PlayOneShot(audioClip);
    }

    public static AudioSource PlaySoundStoppable(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return null;
        }
        AudioSource audioSource = null;
        for (int i = 0; i < 3; i++)
        {
            if (!soundController.audioSources[i].isPlaying)
            {
                audioSource = soundController.audioSources[i];
                break;
            }
        }
        if (audioSource == null)
        {
            audioSource = soundController.gameObject.AddComponent<AudioSource>();
            audioSource.volume = Volume;
            soundController.audioSources.Add(audioSource);
        }
        audioSource.PlayOneShot(audioClip);
        return audioSource;
    }

    private void Update()
    {
        for (int i = 3; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                DestroyImmediate(audioSources[i]);
                audioSources.RemoveAt(i);
            }
        }
    }
}
