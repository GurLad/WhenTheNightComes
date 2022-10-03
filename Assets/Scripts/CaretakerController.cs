using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaretakerController : MonoBehaviour
{
    public static CaretakerController Current;
    private List<Caretaker> caretakers = new List<Caretaker>();
    private List<InteractableObject> queue = new List<InteractableObject>();

    private void Awake()
    {
        Current = this;
    }

    private void Update()
    {
        Caretaker caretaker;
        if (queue.Count > 0 && (caretaker = GetClosestAvailableCaretaker(queue[0], false)) != null)
        {
            queue[0].SelectCaretaker(caretaker);
            queue.RemoveAt(0);
        }
    }

    public void AddCaretaker(Caretaker caretaker)
    {
        caretakers.Add(caretaker);
    }

    public Caretaker GetClosestAvailableCaretaker(InteractableObject pos, bool shouldQueue = true)
    {
        if (caretakers.Count <= 0)
        {
            throw new System.Exception("No caretakers!");
        }
        List<Caretaker> availableCaretakers = caretakers.FindAll(a => a.Available);
        if (availableCaretakers.Count <= 0)
        {
            if (shouldQueue)
            {
                queue.Add(pos);
            }
            return null;
        }
        Caretaker minCaretaker = availableCaretakers[0];
        float minDistance = Pathfinder.GetTrueDistance(minCaretaker.transform.position.To2D(), pos.CaretakerPos);
        for (int i = 1; i < availableCaretakers.Count; i++)
        {
            float distance = Pathfinder.GetTrueDistance(availableCaretakers[i].transform.position.To2D(), pos.CaretakerPos);
            if (distance < minDistance)
            {
                minDistance = distance;
                minCaretaker = availableCaretakers[i];
            }
        }
        return minCaretaker;
    }
}
