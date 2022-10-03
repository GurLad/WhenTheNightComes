using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    private enum SweepState { None, FirstPass, SecondPass, Return }
    [Header("General")]
    public float ArrivalThreshold = 0.1f;
    public Flashlight Flashlight;
    public AudioClip WalkSFX;
    public AudioClip RunSFX;
    public List<AudioClip> OnCallSFX;
    public List<AudioClip> OnArriveSFX;
    [Header("Minimap")]
    public Renderer MinimapIndicator;
    public Color MinimapIdling;
    public Color MinimapActive;
    [Header("Run")]
    public CaretakerStats RunStats;
    [Header("Idle")]
    public CaretakerStats IdleStats;
    public Vector2 IdlePauseTimeRange;
    public Vector2Int IdleMoveRange;
    public Transform RotationObject;
    [HideInInspector]
    public bool Available;
    private Rigidbody rigidbody;
    private CaretakerStats stats;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private SweepState sweepState = SweepState.None;
    private float count;
    private float idlePauseTime = -1;
    private float previousYRot;
    private float targetYRot;
    private float currentYRot;
    private Vector2Int? lookAtPos;
    private AudioSource3D walkingAudioSource;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        CaretakerController.Current.AddCaretaker(this);
        Available = true;
        MinimapIndicator.material = Instantiate(MinimapIndicator.material);
        MinimapIndicator.material.color = MinimapIdling;
        //// DEBUG
        //transform.position = new Vector2Int(19, 13).To3D();
        //SetTarget(new Vector2Int(16, 17), false);
    }

    private void Update()
    {
        if (UIManager.Current.IsAnyWindowOpen())
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        if (currentPath.Count > 0) // Moving
        {
            FollowPath();
            return;
        }
        if (sweepState != SweepState.None) // Sweeping
        {
            if (Rotate(sweepState == SweepState.SecondPass ? stats.SweepSpeed / 2 : stats.SweepSpeed))
            {
                switch (sweepState)
                {
                    case SweepState.None:
                        break;
                    case SweepState.FirstPass:
                        //Debug.Log("Began second pass");
                        count = 0;
                        sweepState = SweepState.SecondPass;
                        previousYRot = currentYRot;
                        targetYRot = currentYRot + stats.SweepArc;
                        break;
                    case SweepState.SecondPass:
                        //Debug.Log("Began return");
                        count = 0;
                        sweepState = SweepState.Return;
                        previousYRot = currentYRot;
                        targetYRot = currentYRot - stats.SweepArc / 2;
                        break;
                    case SweepState.Return:
                        //Debug.Log("Finished sweep");
                        Flashlight.Active = false;
                        count = 0;
                        sweepState = SweepState.None;
                        Available = true;
                        MinimapIndicator.material.color = MinimapIdling;
                        break;
                    default:
                        break;
                }
            }
            RotationObject.transform.localEulerAngles = new Vector3(0, currentYRot, 0);
            return;
        }
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
        if (walkingAudioSource == null || walkingAudioSource.AudioSource == null || !walkingAudioSource.AudioSource.isPlaying)
        {
            if (walkingAudioSource == null || walkingAudioSource.AudioSource == null)
            {
                //Debug.Log("Null");
            }
            walkingAudioSource = SoundController.Play3DSound(Available ? WalkSFX : RunSFX, gameObject);
        }
        if (Vector3.Distance(transform.position - new Vector3(0, transform.position.y, 0), currentPath[0].To3D()) <= ArrivalThreshold)
        {
            currentPath.RemoveAt(0);
            if (currentPath.Count <= 0)
            {
                rigidbody.velocity = Vector3.zero;
                count = 0;
                // Begin sweep
                Flashlight.Active = true;
                sweepState = SweepState.FirstPass;
                previousYRot = currentYRot;
                if (lookAtPos == null)
                {
                    targetYRot = currentYRot - stats.SweepArc / 2;
                }
                else
                {
                    targetYRot = GetLookAtRot(transform.position.To2D(), lookAtPos ?? throw new System.Exception("Impossible")) - stats.SweepArc / 2;
                    lookAtPos = null;
                }
                if (walkingAudioSource != null)
                {
                    walkingAudioSource.AudioSource.Stop();
                }
                if (!Available)
                {
                    SoundController.Play3DSound(OnArriveSFX[Random.Range(0, OnArriveSFX.Count)], gameObject);
                }
                //Debug.Log("Began first pass");
                return;
            }
            GenerateRots();
        }
        Rotate();
        RotationObject.transform.localEulerAngles = new Vector3(0, currentYRot, 0);
        Vector3 dir = -(transform.position - currentPath[0].To3D());
        dir.y = 0;
        rigidbody.velocity = dir.normalized * stats.Speed;
    }

    private bool Rotate(float speed = -1)
    {
        if (count < 1)
        {
            currentYRot = Mathf.Lerp(previousYRot, targetYRot, -(Mathf.Sin(count * Mathf.PI + Mathf.PI / 2)) / 2 + 0.5f);
            count += Time.deltaTime * (speed < 0 ? stats.TurnSpeed : speed);
            return false;
        }
        else
        {
            currentYRot = targetYRot;
            return true;
        }
    }

    private void GenerateRots(Vector2Int start, Vector2Int lookAt)
    {
        previousYRot = currentYRot;
        if (Mathf.Abs(previousYRot) > 180) // Bind to 180
        {
            previousYRot -= Mathf.Sign(previousYRot) > 0 ? 360 : -360;
        }
        targetYRot = GetLookAtRot(start, lookAt);
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
        //Debug.Log(previousYRot + " -> " + targetYRot);
        count = 0;
    }

    private void GenerateRots()
    {
        GenerateRots(transform.position.To2D(), currentPath[0]);
    }

    private float GetLookAtRot(Vector2Int start, Vector2Int lookAt)
    {
        Vector2 diff = new Vector2((start - lookAt).x, (start - lookAt).y).normalized;
        return Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    }

    public void SetTarget(Vector2Int pos, Vector2Int lookAtPos) // If looking at something, always running
    {
        SetTarget(pos, true);
        this.lookAtPos = lookAtPos;
    }

    public void SetTarget(Vector2Int pos, bool run = true)
    {
        //Debug.Log("Target");
        // Immediatly stop current action
        currentPath.Clear();
        //transform.position = transform.position.To2D().To3D() + new Vector3(0, transform.position.y, 0);
        stats = run ? RunStats : IdleStats;
        if (transform.position.To2D() == pos)
        {
            rigidbody.velocity = Vector3.zero;
            count = 0;
            // Begin sweep
            Flashlight.Active = true;
            sweepState = SweepState.FirstPass;
            previousYRot = currentYRot;
            if (lookAtPos == null)
            {
                targetYRot = currentYRot - stats.SweepArc / 2;
            }
            else
            {
                targetYRot = GetLookAtRot(transform.position.To2D(), lookAtPos ?? throw new System.Exception("Impossible")) - stats.SweepArc / 2;
                lookAtPos = null;
            }
            if (walkingAudioSource != null)
            {
                walkingAudioSource.AudioSource.Stop();
            }
            if (!Available)
            {
                SoundController.Play3DSound(OnArriveSFX[Random.Range(0, OnArriveSFX.Count)], gameObject);
            }
            //Debug.Log("Began first pass");
        }
        else
        {
            currentPath = Pathfinder.GetPath(transform.position.To2D(), pos);
            currentPath.RemoveAt(0); // No need for the start pos
            GenerateRots();
        }
        if (run)
        {
            Available = false;
            MinimapIndicator.material.color = MinimapActive;
            SoundController.Play3DSound(OnCallSFX[Random.Range(0, OnCallSFX.Count)], gameObject);
        }
    }

    [System.Serializable]
    public class CaretakerStats
    {
        public float Speed;
        public float TurnSpeed;
        public float SweepSpeed;
        public float SweepArc = 60;
    }
}
