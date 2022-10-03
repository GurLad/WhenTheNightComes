using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string Name;

    public void Click()
    {
        SceneManager.LoadScene(Name); // TEMP
    }
}
