using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public string Name;
    public int a;

    private void Start()
    {
        CrossfadeMusicPlayer.Instance.Play(Name);
    }
}
