using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSource3D : MonoBehaviour
{
    public AudioListener Listener;
    public AudioSource AudioSource;
    public float MinValue;
    public Vector2 Range;

    public void Init(AudioListener listener, AudioSource audioSource, float minValue, Vector2 range)
    {
        Listener = listener;
        AudioSource = audioSource;
        MinValue = minValue;
        Range = range;
    }

    public void CalcVolume()
    {
        Vector2 finalRange = Range;
        float dist = Vector3.Distance(Listener.transform.position, transform.position);
        AudioSource.volume = SoundController.Volume * Mathf.Clamp(1 + (finalRange.x - dist) / (finalRange.y - finalRange.x), 0, 1);
        //Debug.Log("Obj: " + gameObject.name + ", dist: " + dist + ", volume: " + AudioSource.volume);
    }
}
