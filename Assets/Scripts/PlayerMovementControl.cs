using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : MonoBehaviour
{
    public float MovementSpeed;
    public float SidewaysSpeedMultiplier;
    public float BackwardsSpeedMultiplier;

    private CharacterController CharController;
    private Vector3 Direction, CameraRotation;

    private SettingsController SC;
    private void Start()
    {
        CharController = this.GetComponent<CharacterController>();
        SC = FindObjectOfType<SettingsController>();
    }
    void Update()
    {
        if (!SC.SettingsActive)
        {
            Direction = Vector3.zero;
            if (Input.GetKey("w") && !Input.GetKey("s"))
            {
                Direction += new Vector3(0, 0, 1);
            }
            if (Input.GetKey("a") && !Input.GetKey("d"))
            {
                Direction += new Vector3(-1, 0, 0) * SidewaysSpeedMultiplier;
            }
            if (Input.GetKey("s") && !Input.GetKey("w"))
            {
                Direction += new Vector3(0, 0, -1) * BackwardsSpeedMultiplier;
            }
            if (Input.GetKey("d") && !Input.GetKey("a"))
            {
                Direction += new Vector3(1, 0, 0) * SidewaysSpeedMultiplier;
            }

            Direction = Vector3.Normalize(Direction);

            CameraRotation = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            Direction = Quaternion.Euler(CameraRotation) * Direction;

            CharController.Move(Direction * MovementSpeed * Time.deltaTime);
        }
    }
}
