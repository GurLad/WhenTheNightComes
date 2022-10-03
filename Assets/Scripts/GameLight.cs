using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLight : MonoBehaviour
{
    public Light Light;

    private void Reset()
    {
        Light = GetComponent<Light>();
    }

    private void Start()
    {
        LightController.Current.GameLights.Add(this);
    }
}
