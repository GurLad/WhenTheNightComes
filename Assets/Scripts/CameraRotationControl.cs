using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationControl : MonoBehaviour
{
    public float Sensitivity;
    private Quaternion OldRotation, Offset, NewRotation;

    private SettingsController SC;

    private void Start()
    {
        SC = FindObjectOfType<SettingsController>();
    }

    void Update() //Calculating new camera angles based on mouse movement
    {
        if (!SC.SettingsActive)
        {
            OldRotation = transform.rotation;
            Offset = Quaternion.Euler(-Input.GetAxis("Mouse Y") * Sensitivity, Input.GetAxis("Mouse X") * Sensitivity, 0);
            NewRotation = Quaternion.Euler(OldRotation.eulerAngles + Offset.eulerAngles);
            transform.rotation = NewRotation;
        }
    }
}
