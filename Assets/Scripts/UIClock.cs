using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClock : MonoBehaviour
{
    private static UIClock _current;
    public static UIClock Current
    {
        get
        {
            return _current != null ? _current : (_current = FindObjectOfType<UIClock>());
        }
    }
    private float ClockTime, LastTime;
    public AudioClip Bell, Ticking;
    public GameObject MinHandle, HourHandle;
    public bool ClockTicking;

    private UIManager UIM;

    void Start()
    {
        ClockTime = 0;
        UIM = FindObjectOfType<UIManager>();
        //SoundController.PlaySound(Ticking);
        Conductor.PlaySong("Level");
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIM.IsAnyWindowOpen())
        {
            LastTime = ClockTime;
            if (ClockTicking)
            {
                Debug.Log("Time since last: " + Conductor.TimeSinceLastBeat + ", pos: " + Conductor.SongPositionInBeats);
                ClockTime = Conductor.TimeSinceLastBeat + Conductor.SongPositionInBeats;
            }

            MinHandle.transform.rotation = Quaternion.Euler(0, 0, -ClockTime % 10.0f * 36);

            HourHandle.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Floor(ClockTime / 10.0f) * 30+60);

            if(Mathf.Floor(LastTime / 10.0f)!= Mathf.Floor(ClockTime / 10.0f))
            {
                //SoundController.PlaySound(Bell);
                //SoundController.PlaySound(Ticking);
            }
        }

        if (ClockTime > 90)
            Win();
    }

    public int CurrentHour() 
    {
        return (int)Mathf.Floor(ClockTime / 10.0f);
    }

    public float CurrentTime()
    {
        return ClockTime;
    }

    public float CurrentSecond()
    {
        return ClockTime % 10.0f;
    }

    public void SetTime(float Time_)
    {
        ClockTime = Time_;
    }

    private void Win() 
    {
        UIM.OpenWindow(UIManager.UIElements.NextLevel);
        FindObjectOfType<ScoreManager>().EndLevel();
    }
}
