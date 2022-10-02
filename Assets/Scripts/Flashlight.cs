using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject Caretaker;
    public bool Active;

    private void OnTriggerEnter(Collider other)
    {
        MonsterController monster = other.gameObject.GetComponentInParent<MonsterController>(); // Assume the hitbox is of the bed model
        if (monster != null && Active)
        {
            // Check line of sight just to be sure
            RaycastHit hit;
            var rayDirection = Caretaker.transform.position - other.transform.position;
            if (Physics.Raycast(other.transform.position, rayDirection, out hit))
            {
                if (hit.transform == Caretaker.transform)
                {
                    Debug.Log("Hit monster");
                    monster.StopMonsterAttack();
                }
                else
                {
                    Debug.Log("No line of sight!");
                }
            }
        }
    }
}
