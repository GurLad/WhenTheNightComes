using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    [Header("General")]
    public float ArrivalThreshold = 0.1f;
    [Header("Run")]
    public CaretakerStats RunStats;
    [Header("Idle")]
    public CaretakerStats IdleStats;
    public Vector2 IdlePauseTimeRange;
    public Vector2Int IdleMoveRange;
    public Transform RotationObject;
    private Rigidbody rigidbody;
    private CaretakerStats stats;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private float count;
    private float idlePauseTime = -1;
    private float previousYRot;
    private float targetYRot;
    private float currentYRot;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //// DEBUG
        //transform.position = new Vector2Int(19, 13).To3D();
        //SetTarget(new Vector2Int(16, 17), false);
    }

    private void Update()
    {
        if (currentPath.Count > 0) // Moving
        {
            FollowPath();
            return;
        }
        //if (SwipeAnimation.Active) // Swiping
        //{
        //    return;
        //}
        // Idling
        if (idlePauseTime < 0)
        {
            idlePauseTime = Random.Range(IdlePauseTimeRange.x, IdleMoveRange.y);
        }
        count += Time.deltaTime;
        if (count >= idlePauseTime)
        {
            count = 0;
            idlePauseTime = -1;
            // Select target
            int move = Random.Range(IdleMoveRange.x, IdleMoveRange.y + 1);
            while (move >= 1 && currentPath.Count <= 0)
            {
                // Try a different amount of rotations depending on the move range
                int rots = (int)Mathf.Pow(2, move + 1); // Might scale too quickly
                int baseRot = Random.Range(0, rots);
                float baseRotRads = baseRot / (float)rots * 2 * Mathf.PI;
                for (int i = 0; i < rots; i++)
                {
                    float iRads = i / (float)rots * 2 * Mathf.PI;
                    //Debug.Log("Move: " + move + ", baseRot: " + baseRot + " -> " + baseRotRads + " rads, i: " + i + " -> " + iRads);
                    Vector2 baseTarget = new Vector2(Mathf.Sin(baseRotRads + iRads), Mathf.Cos(baseRotRads + iRads)) * move;
                    Vector2Int target = transform.position.To2D() + new Vector2Int(Mathf.RoundToInt(baseTarget.x), Mathf.RoundToInt(baseTarget.y));
                    //Debug.Log("Trying " + baseTarget + ": " + transform.position.To2D() + " -> " + target);
                    if (Pathfinder.HasLineOfSight(transform.position.To2D(), target))
                    {
                        //Debug.Log("Chose");
                        SetTarget(target, false);
                        return;
                    }
                }
                move--;
                //Debug.Log("Failed, trying " + move + " move");
            }
            Debug.LogWarning("Can't move!");
        }
    }

    private void FollowPath()
    {
        if (Vector3.Distance(transform.position - new Vector3(0, transform.position.y, 0), currentPath[0].To3D()) <= ArrivalThreshold)
        {
            currentPath.RemoveAt(0);
            if (currentPath.Count <= 0)
            {
                rigidbody.velocity = Vector3.zero;
                count = 0;
                //SwipeAnimation.Activate();
                return;
            }
            GenerateRots();
        }
        if (count < 1)
        {
            currentYRot = Mathf.Lerp(previousYRot, targetYRot, -(Mathf.Sin(count * Mathf.PI + Mathf.PI / 2)) / 2 + 0.5f);
            count += Time.deltaTime * stats.TurnSpeed;
        }
        else
        {
            currentYRot = targetYRot;
        }
        RotationObject.transform.localEulerAngles = new Vector3(0, currentYRot, 0);
        Vector3 dir = -(transform.position - currentPath[0].To3D());
        dir.y = 0;
        rigidbody.velocity = dir.normalized * stats.Speed;
    }

    private void GenerateRots()
    {
        previousYRot = currentYRot;
        if (Mathf.Abs(previousYRot) > 180) // Bind to 180
        {
            previousYRot -= Mathf.Sign(previousYRot) > 0 ? 360 : -360;
        }
        Vector2 diff = new Vector2((transform.position.To2D() - currentPath[0]).x, (transform.position.To2D() - currentPath[0]).y).normalized;
        targetYRot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(targetYRot - previousYRot) > 180) // Rotating the opposite direction
        {
            if (targetYRot < 0)
            {
                targetYRot += 360;
            }
            else // previousYRot < 0 - atan bounds to (-180,180)
            {
                previousYRot += 360;
            }
        }
        Debug.Log(previousYRot + " -> " + targetYRot);
        count = 0;
    }

    public void SetTarget(Vector2Int pos, bool run = true)
    {
        //transform.position = transform.position.To2D().To3D() + new Vector3(0, transform.position.y, 0);
        stats = run ? RunStats : IdleStats;
        currentPath = Pathfinder.GetPath(transform.position.To2D(), pos);
        currentPath.RemoveAt(0); // No need for the start pos
        GenerateRots();
    }

    [System.Serializable]
    public class CaretakerStats
    {
        public float Speed;
        public float TurnSpeed;
        public float SwipeSpeed;
    }
}
