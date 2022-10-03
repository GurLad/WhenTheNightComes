using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OADrop : OAParticles
{
    private enum State { None, Falling, Recovering }
    public GameObject Rotator;
    public float FallSpeed;
    public float RecoverSpeed;
    private State state;
    private float count;
    // Add SFX

    public override void AnimateHighlight()
    {
        base.AnimateHighlight();
    }

    public override void AnimateRecovery()
    {
        base.AnimateRecovery();
        count = 0;
        state = State.Recovering;
    }

    public override void AnimateInteraction()
    {
        base.AnimateInteraction();
        count = 0;
        state = State.Falling;
    }

    public override void AnimateStopHighlight()
    {
        base.AnimateStopHighlight();
    }

    private void Update()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Falling:
                count += Time.deltaTime * FallSpeed;
                if (count >= 1)
                {
                    count = 0;
                    state = State.None;
                    Rotator.transform.localEulerAngles = new Vector3(-90, 0, 0);
                }
                else
                {
                    Rotator.transform.localEulerAngles = new Vector3(-90 * Mathf.Pow(count, 2), 0, 0);
                }
                break;
            case State.Recovering:
                count += Time.deltaTime * RecoverSpeed;
                if (count >= 1)
                {
                    count = 0;
                    state = State.None;
                    Rotator.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    Rotator.transform.localEulerAngles = new Vector3(-90 * Mathf.Pow(1 - count, 2), 0, 0);
                }
                break;
            default:
                break;
        }
    }
}
