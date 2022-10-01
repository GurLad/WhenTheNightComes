using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    private Rigidbody rigidbody;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private float count;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        SetTarget(new Vector2Int(3, 3));
    }

    private void Update()
    {
        if (currentPath.Count > 0)
        {
            // TEMP LAZY IMPLEMENTATION
            count += Time.deltaTime;
            if (count > 0.5f)
            {
                transform.position = currentPath[0].To3D() + new Vector3(0, transform.position.y, 0);
                currentPath.RemoveAt(0);
                count--;
            }
        }
    }

    public void SetTarget(Vector2Int pos)
    {
        currentPath = Pathfinder.GetPath(transform.position.To2D(), pos);
    }
}
