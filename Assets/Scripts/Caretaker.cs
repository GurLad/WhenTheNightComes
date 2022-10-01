using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    public float Speed;
    public float ArrivalThreshold = 0.1f;
    public float TurnSpeed;
    public Transform RotationObject;
    private Rigidbody rigidbody;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private float count;
    private Quaternion currentRot;
    private Quaternion targetRot;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        SetTarget(new Vector2Int(3, 3));
    }

    private void Update()
    {
        if (currentPath.Count > 0)
        {
            if (Vector3.Distance(transform.position - new Vector3(0, transform.position.y, 0), currentPath[0].To3D()) <= ArrivalThreshold)
            {
                currentPath.RemoveAt(0);
                if (currentPath.Count <= 0)
                {
                    rigidbody.velocity = Vector3.zero;
                    return;
                }
                currentRot = RotationObject.transform.localRotation;
                RotationObject.transform.LookAt(-(RotationObject.transform.position - currentPath[0].To3D()));
                targetRot = RotationObject.transform.localRotation;
                count = 0;
            }
            if (count < 1)
            {
                RotationObject.transform.localRotation = Quaternion.Slerp(currentRot, targetRot, count);
                count += Time.deltaTime * TurnSpeed;
            }
            else
            {
                RotationObject.transform.localRotation = targetRot;
            }
            Vector3 dir = -(transform.position - currentPath[0].To3D());
            dir.y = 0;
            rigidbody.velocity = dir.normalized * Speed;
        }
    }

    public void SetTarget(Vector2Int pos)
    {
        currentPath = Pathfinder.GetPath(transform.position.To2D(), pos);
    }
}
