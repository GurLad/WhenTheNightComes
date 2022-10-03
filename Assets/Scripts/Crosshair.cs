using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private static Crosshair _current;
    public static Crosshair Current
    {
        get
        {
            return _current != null ? _current : (_current = FindObjectOfType<Crosshair>());
        }
    }
    public Image Image;
    public Sprite Base;
    public Sprite HandS;

    public void Hand()
    {
        Image.sprite = HandS;
    }

    public void NoHand()
    {
        Image.sprite = Base;
    }
}
