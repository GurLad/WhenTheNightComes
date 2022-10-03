using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private enum State { None, OffFirst, OnFirst, OffSecond }
    private static LightController _current;
    public static LightController Current
    {
        get
        {
            return _current != null ? _current : (_current = FindObjectOfType<LightController>());
        }
    }
    public float FlickerLength;
    public float FlickerPause;
    [HideInInspector]
    public List<GameLight> GameLights = new List<GameLight>();
    private int previousHour;
    private State state;
    private float count;

    private void Update()
    {
        if (previousHour != UIClock.Current.CurrentHour())
        {
            previousHour = UIClock.Current.CurrentHour();
            GameLights.ForEach(a => a.Light.enabled = false);
            count = 0;
            state = State.OffFirst;
        }
        switch (state)
        {
            case State.None:
                break;
            case State.OffFirst:
                count += Time.deltaTime;
                if (count >= FlickerLength)
                {
                    GameLights.ForEach(a => a.Light.enabled = true);
                    count = 0;
                    state = State.OnFirst;
                }
                break;
            case State.OnFirst:
                count += Time.deltaTime;
                if (count >= FlickerPause)
                {
                    GameLights.ForEach(a => a.Light.enabled = false);
                    count = 0;
                    state = State.OffSecond;
                }
                break;
            case State.OffSecond:
                count += Time.deltaTime;
                if (count >= FlickerLength)
                {
                    GameLights.ForEach(a => a.Light.enabled = true);
                    count = 0;
                    state = State.None;
                }
                break;
            default:
                break;
        }
    }
}
