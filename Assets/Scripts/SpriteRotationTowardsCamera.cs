using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotationTowardsCamera : MonoBehaviour
{
    public float offset = 0;
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y+offset, 0);
    }
}
