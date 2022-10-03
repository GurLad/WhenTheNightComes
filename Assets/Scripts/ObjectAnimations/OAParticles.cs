using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAParticles : ObjectAnimation
{
    public ParticleSystem ParticleSystem;
    public AudioClip[] ClickSFX;

    protected virtual void Start()
    {
        if (ParticleSystem.isPlaying)
        {
            ParticleSystem.Stop();
        }
    }

    public override void AnimateHighlight()
    {
        if (!ParticleSystem.isPlaying)
        {
            ParticleSystem.Play();
        }
    }

    public override void AnimateInteraction()
    {
        SoundController.PlaySound(ClickSFX[Random.Range(0, ClickSFX.Length)]);
        if (ParticleSystem.isPlaying)
        {
            ParticleSystem.Stop();
        }
    }

    public override void AnimateRecovery()
    {
        // Do nothing
    }

    public override void AnimateStopHighlight()
    {
        if (ParticleSystem.isPlaying)
        {
            ParticleSystem.Stop();
        }
    }
}
