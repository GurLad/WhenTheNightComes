using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationControl : MonoBehaviour
{
    public float Sensitivity;
    private Quaternion OldRotation, Offset, NewRotation;

    public float ObjectInteractionMaxDistance;

    private UIManager UIM;

    private void Start()
    {
        UIM = FindObjectOfType<UIManager>();
    }

    void Update() //Calculating new camera angles based on mouse movement
    {
        if (!UIM.IsAnyWindowOpen())
        {
            OldRotation = transform.rotation;
            Offset = Quaternion.Euler(-Input.GetAxis("Mouse Y") * Sensitivity, Input.GetAxis("Mouse X") * Sensitivity, 0);
            NewRotation = Quaternion.Euler(OldRotation.eulerAngles + Offset.eulerAngles);
            transform.rotation = NewRotation;

            Raycast();
        }
    }

    private void Raycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.5f,0.5f,0));

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if(Vector3.Distance(Camera.main.transform.position, hit.transform.position)<ObjectInteractionMaxDistance) //checking if object is in range
            {
                if (hit.transform.gameObject.GetComponent<InteractableObject>() != null)
                {
                    hit.transform.gameObject.GetComponent<InteractableObject>().Highlight();
                    if(Input.GetMouseButtonDown(0))
                    {
                        hit.transform.gameObject.GetComponent<InteractableObject>().Interact();
                    }
                }
            }
        }
    }
}
