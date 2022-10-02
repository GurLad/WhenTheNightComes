using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEyes : MonoBehaviour
{
    private SpriteRenderer SR;

    public float alpha = 0, BlinkStart = -999.0f, TimeSinceStartedBlinking = 1000.0f;

    public float BlinkDuration;

    void Start()
    {
        SR = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //if (Random.value < 0.001) //Testing only!
        //    Blink();

        TimeSinceStartedBlinking = Time.time - BlinkStart;
        if(TimeSinceStartedBlinking<BlinkDuration)
        {
            alpha = Mathf.Round(-255 * TimeSinceStartedBlinking * (TimeSinceStartedBlinking - BlinkDuration));
        }
        else
        {
            alpha = 0;
        }

        SR.color = new Color32(255, 255, 255, (byte)alpha);
    }

    public void Blink()
    {
        if(Time.time-BlinkStart>BlinkDuration)
        {
            BlinkStart = Time.time;
        }
    }

    public bool IsBlinking()
    {
        if (Time.time - BlinkStart > BlinkDuration+0.75f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
