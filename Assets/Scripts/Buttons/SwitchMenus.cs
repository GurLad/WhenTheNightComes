using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenus : MonoBehaviour
{
    public GameObject This;
    public GameObject That;

    public void Click()
    {
        This.SetActive(false);
        That.SetActive(true);
    }
}
