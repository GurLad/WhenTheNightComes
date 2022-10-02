using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectAnimation : MonoBehaviour
{
    public abstract void AnimateInteraction(); // Animation once a player interacts with the object to call the caretakers
    public abstract void AnimateRecovery(); // Animation once the object recovers (every 10 seconds? When caretakers arrive?)
}
