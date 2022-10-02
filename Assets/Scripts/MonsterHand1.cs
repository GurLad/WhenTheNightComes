using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHand : MonoBehaviour
{
    private bool Active = false;
    public GameObject MonsterHandObject;
    public bool Side = true;
    public float MonsterHandSpeed;
    public float HandPhase, HandFrequency, HandAmplitude;
    private float Angle=0, RandomAngle, BaseAngle, TotalAngle;
    void Start()
    {
        HandPhase += 4*Mathf.PI * (Random.value - 0.5f);
        HandFrequency *= 1.0f+(Random.value-0.5f)/5.0f;
        HandAmplitude *= 1.0f + (Random.value - 0.5f) / 5.0f;
        BaseAngle = this.transform.localRotation.eulerAngles.y;

        Angle = -90.0f;
    }

    void Update()
    {

        //if (Time.realtimeSinceStartup > 10.0) //for testing!
        //    Active = true;

       
        if (Active && Angle  < 0)
            Angle += Time.deltaTime * MonsterHandSpeed;
        if (!Active && Angle > -90)
            Angle -= Time.deltaTime * MonsterHandSpeed;

        Angle = Mathf.Clamp(Angle,-90.0f,0.0f);

        RandomAngle = HandAmplitude * Mathf.Sin(HandFrequency * Time.time + HandPhase);

        if (Angle < -60)
            RandomAngle = 0;

        TotalAngle = BaseAngle + Angle + RandomAngle;

        if (!Side)
            TotalAngle *= -1;

        this.transform.localRotation = Quaternion.Euler(0,TotalAngle , 0);

        if (Angle < -80.0f&&!Active)
            MonsterHandObject.SetActive(false);
        else
            MonsterHandObject.SetActive(true);

    }

    public void Show()
    {
        Active = true;
    }

    public void Hide()
    {
        Active = false;
    }

    
}
