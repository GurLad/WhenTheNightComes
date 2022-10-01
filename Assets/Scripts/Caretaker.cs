using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    public float Speed;
    public float ArrivalThreshold = 0.1f;
    public float TurnSpeed;
    private Rigidbody rigidbody;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private float count;
    private float yRot;
    private float targetRot;

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
                // Adapted from https://answers.unity.com/questions/1023987/lookat-only-on-z-axis.html
                Vector3 difference = currentPath[0].To3D() - transform.position;
                targetRot = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg;
            }
            if (Mathf.Abs(targetRot - yRot) >= ArrivalThreshold)
            {
                yRot += Mathf.Sign(targetRot - yRot) * Time.deltaTime * TurnSpeed;
            }
            else
            {
                yRot = targetRot;
            }
            transform.localEulerAngles = new Vector3(0, yRot, 0);
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
