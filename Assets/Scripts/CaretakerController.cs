using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaretakerController : MonoBehaviour
{
    public static CaretakerController Current;
    private List<Caretaker> caretakers = new List<Caretaker>();

    private void Awake()
    {
        Current = this;
    }

    public void AddCaretaker(Caretaker caretaker)
    {
        caretakers.Add(caretaker);
    }

    public Caretaker GetClosestAvailableCaretaker(Vector2Int point, bool mustBeAvailabe = true)
    {
        if (caretakers.Count <= 0)
        {
            throw new System.Exception("No caretakers!");
        }
        List<Caretaker> availableCaretakers = mustBeAvailabe ? caretakers.FindAll(a => a.Available) : caretakers;
        if (availableCaretakers.Count <= 0)
        {
            return null;
        }
        Caretaker minCaretaker = availableCaretakers[0];
        float minDistance = Pathfinder.GetTrueDistance(minCaretaker.transform.position.To2D(), point);
        for (int i = 1; i < availableCaretakers.Count; i++)
        {
            float distance = Pathfinder.GetTrueDistance(availableCaretakers[i].transform.position.To2D(), point);
            if (distance < minDistance)
            {
                minDistance = distance;
                minCaretaker = availableCaretakers[i];
            }
        }
        return minCaretaker;
    }
}
