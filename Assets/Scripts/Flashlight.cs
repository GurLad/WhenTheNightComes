using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject Caretaker;
    public Renderer MinimapFlashlight;
    public Color ActiveColor;
    public Color InactiveColor;
    private Material minimapMaterial;
    private bool _active;
    public bool Active
    { 
        get
        {
            return _active;
        }
        set
        {
            _active = value;
            minimapMaterial.color = Active ? ActiveColor : InactiveColor;
        }
    }

    private void Start()
    {
        minimapMaterial = MinimapFlashlight.material = Instantiate(MinimapFlashlight.material);
    }

    private void OnTriggerStay(Collider other)
    {
        MonsterController monster = other.gameObject.GetComponentInParent<MonsterController>(); // Assume the hitbox is of the bed model
        if (monster != null && Active)
        {
            // Check line of sight just to be sure
            RaycastHit hit;
            var rayDirection = Caretaker.transform.position - other.transform.position;
            if (Physics.Raycast(other.transform.position, rayDirection, out hit, Mathf.Infinity, ~(1 << 7)))
            {
                if (hit.transform == Caretaker.transform)
                {
                    //Debug.Log("Hit monster");
                    monster.StopMonsterAttack();
                }
                else
                {
                    //Debug.Log("No line of sight! Hit " + hit.transform.name);
                }
            }
        }
    }
}
