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

    public Caretaker GetClosestCaretaker(Vector2Int point)
    {
        if (caretakers.Count <= 0)
        {
            throw new System.Exception("No caretakers!");
        }
        Caretaker minCaretaker = caretakers[0];
        float minDistance = Pathfinder.GetTrueDistance(minCaretaker.transform.position.To2D(), point);
        for (int i = 1; i < caretakers.Count; i++)
        {
            float distance = Pathfinder.GetTrueDistance(caretakers[i].transform.position.To2D(), point);
            if (distance < minDistance)
            {
                minDistance = distance;
                minCaretaker = caretakers[i];
            }
        }
        return minCaretaker;
    }
}
