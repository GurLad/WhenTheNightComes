using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaretakerRotator : MonoBehaviour
{
    public Renderer Renderer;
    public Texture Forward;
    public Texture Left;

    private void Start()
    {
        Renderer.material = Instantiate(Renderer.material);
    }

    private void Update()
    {
        int rot90 = Mathf.FloorToInt(((transform.localEulerAngles.y + 360) % 360) / 90);
        Renderer.material.SetTextureScale("_MainTex", new Vector2(rot90 >= 2 ? 1 : -1, 1)); // Flip
        Renderer.material.mainTexture = rot90 % 2 == 0 ? Forward : Left;
    }
}
