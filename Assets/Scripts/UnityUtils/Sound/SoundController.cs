using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.SocialPlatforms;

public class SoundController : MonoBehaviour
{
    private static AudioListener _listener;
    private static AudioListener listener
    {
        get
        {
            return _listener != null ? _listener : (_listener = FindObjectOfType<AudioListener>());
        }
    }
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
    [SerializeField]
    private Vector2 base3DRange; // Closer than x is 1, further than y is 0, middle is middle
    private List<AudioSource> audioSources = new List<AudioSource>();
    private List<AudioSource3D> audioSources3D = new List<AudioSource3D>();
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
        soundController.audioSources3D.RemoveAll(a => a.AudioSource == audioSource);
        audioSource.volume = Volume;
        audioSource.PlayOneShot(audioClip);
        return audioSource;
    }

    public static AudioSource Play3DSound(AudioClip audioClip, GameObject obj, float minValue = 0, Vector2? range = null)
    {
        if (audioClip == null)
        {
            return null;
        }
        AudioSource audioSource = null;
        for (int i = 0; i < 3; i++)
        {
            if (!soundController.audioSources[i].isPlaying && soundController.audioSources3D.Find(a => a.AudioSource == soundController.audioSources[i]) == null)
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
        // Process volume
        AudioSource3D audioSource3D = obj.AddComponent<AudioSource3D>();
        audioSource3D.Init(listener, audioSource, minValue, range ?? soundController.base3DRange);
        audioSource3D.CalcVolume();
        soundController.audioSources3D.Add(audioSource3D);
        // Play
        audioSource.PlayOneShot(audioClip);
        return audioSource;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < audioSources3D.Count; i++)
        {
            if (!(audioSources3D[i].AudioSource?.isPlaying ?? false))
            {
                Destroy(audioSources3D[i]);
                audioSources3D.RemoveAt(i--);
            }
            else
            {
                audioSources3D[i].CalcVolume();
            }
        }
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
