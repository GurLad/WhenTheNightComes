using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    private void Start()
    {
        if (SetLevel.CurrentLevel > 4)
        {
            SceneLoader.LoadScene("MainMenu");
        }
    }
}
