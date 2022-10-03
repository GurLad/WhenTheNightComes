using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationControl : MonoBehaviour
{
    public float Sensitivity;
    private Quaternion OldRotation, Offset, NewRotation;

    public float ObjectInteractionMaxDistance;

    public GameObject MinimapIndicator;

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
            MinimapIndicator.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            Raycast();
        }
    }

    private void Raycast()
    {
        RaycastHit hit;
        Transform cameraTransform = Camera.main.transform;

        Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, ObjectInteractionMaxDistance);
            

        if (hit.collider!=null)
        {
            Debug.DrawLine(cameraTransform.position, hit.transform.position, Color.yellow);
            GameObject parent = hit.transform.gameObject;

            for (int i = 0; i < 5; i++)
            {
                if (parent.GetComponent<InteractableObject>() != null)
                {
                    parent.GetComponent<InteractableObject>().Highlight();
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        parent.GetComponent<InteractableObject>().Interact();
                    }
                    break;
                }
                else
                {
                    if (parent.transform.parent != null)
                        parent = parent.transform.parent.gameObject;
                    else
                        break;
                }
            }

        }
    }
}
