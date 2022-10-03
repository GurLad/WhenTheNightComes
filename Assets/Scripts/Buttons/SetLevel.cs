using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevel : MonoBehaviour
{
    public static int CurrentLevel; // Bad coding practices here we go!!! :)
    public int Target;
    public bool NextLevel;
    
    public void Click()
    {
        CurrentLevel = NextLevel ? CurrentLevel + 1 : Target;
    }
}
