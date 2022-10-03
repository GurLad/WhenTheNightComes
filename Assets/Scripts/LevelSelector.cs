using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public List<LevelGenerator> Levels;

    private void Awake()
    {
        Levels[SetLevel.CurrentLevel].gameObject.SetActive(true);
    }
}
