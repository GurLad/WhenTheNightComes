using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caretaker : MonoBehaviour
{
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


}
