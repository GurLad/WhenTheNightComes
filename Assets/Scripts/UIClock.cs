using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClock : MonoBehaviour
{
    private float ClockTime, LastTime;
    public GameObject MinHandle, HourHandle;
    public bool ClockTicking;

    private SettingsController SC;

    void Start()
    {
        ClockTime = 0;
        SC = FindObjectOfType<SettingsController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SC.SettingsActive)
        {
            LastTime = ClockTime;
            if (ClockTicking)
                ClockTime += Time.deltaTime;

            MinHandle.transform.rotation = Quaternion.Euler(0, 0, -ClockTime % 10.0f * 36);

            HourHandle.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Floor(ClockTime / 10.0f) * 30);
        }
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


}
