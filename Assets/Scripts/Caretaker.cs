using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    [Header("General")]
    public float ArrivalThreshold = 0.1f;
    public AdvancedAnimation SwipeAnimation;
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
    private Quaternion currentRot;
    private Quaternion targetRot;

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
            currentRot = RotationObject.transform.localRotation;
            RotationObject.transform.LookAt(-(RotationObject.transform.position - currentPath[0].To3D()));
            targetRot = RotationObject.transform.localRotation;
            count = 0;
        }
        if (count < 1)
        {
            RotationObject.transform.localRotation = Quaternion.Slerp(currentRot, targetRot, count);
            count += Time.deltaTime * stats.TurnSpeed;
        }
        else
        {
            RotationObject.transform.localRotation = targetRot;
        }
        Vector3 dir = -(transform.position - currentPath[0].To3D());
        dir.y = 0;
        rigidbody.velocity = dir.normalized * stats.Speed;
    }

    public void SetTarget(Vector2Int pos, bool run = true)
    {
        //transform.position = transform.position.To2D().To3D() + new Vector3(0, transform.position.y, 0);
        stats = run ? RunStats : IdleStats;
        currentPath = Pathfinder.GetPath(transform.position.To2D(), pos);
        currentPath.RemoveAt(0); // No need for the start pos
    }

    [System.Serializable]
    public class CaretakerStats
    {
        public float Speed;
        public float TurnSpeed;
        public float SwipeSpeed;
    }
}
