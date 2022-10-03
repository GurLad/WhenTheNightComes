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
    private int highlightDelay;
    private ObjectAnimation animation;
    private int hourInteracted;

    private void Update()
    {
        if (highlightDelay > 0)
        {
            highlightDelay--; // No need for deltaTime - just checking that didn't highlight this frame
            if (highlightDelay <= 0)
            {
                animation.AnimateStopHighlight();
            }
        }
        if (hourInteracted >= 0 && hourInteracted != UIClock.Current.CurrentHour())
        {
            Recover();
            hourInteracted = -1;
        }
    }

    public void Init(int type = -1)
    {
        CanInteract = true;
        animation = Animations[type >= 0 ? type : Random.Range(0, Animations.Count)];
        animation.gameObject.SetActive(true);
    }

    public void Interact() // Called from PlayerController
    {
        if (CanInteract)
        {
            highlightDelay = 0;
            animation.AnimateInteraction();
            (CaretakerController.Current.GetClosestAvailableCaretaker(CaretakerPos) ??
             CaretakerController.Current.GetClosestAvailableCaretaker(CaretakerPos, false)).SetTarget(CaretakerPos, transform.position.To2D());
            CanInteract = false;
            hourInteracted = UIClock.Current.CurrentHour();
        }
    }

    public void Highlight()
    {
        if (CanInteract)
        {
            animation.AnimateHighlight();
            highlightDelay = 2;
        }
    }

    public void Recover()
    {
        animation.AnimateRecovery();
        CanInteract = true;
    }
}
