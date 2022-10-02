using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public List<ObjectAnimation> Animations;
    [HideInInspector]
    public Vector2Int CaretakerPos;
    [HideInInspector]
    public bool CanInteract;
    private ObjectAnimation animation;

    public void Init(int type = -1)
    {
        animation = Animations[type >= 0 ? type : Random.Range(0, Animations.Count)];
        animation.gameObject.SetActive(true);
    }

    public void Interact() // Called from PlayerController
    {
        if (CanInteract)
        {
            animation.AnimateInteraction();
            CaretakerController.Current.GetClosestCaretaker(transform.position.To2D()).SetTarget(CaretakerPos);
            CanInteract = false;
        }
    }

    public void Highlight()
    {
        // TBA
    }

    public void Recover()
    {
        animation.AnimateRecovery();
        CanInteract = true;
    }
}
