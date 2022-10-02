using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAColor : ObjectAnimation
{
    public Renderer Renderer;
    public Material Material;
    public Color Default;
    public Color Selected;
    public Color Activated;

    private void Start()
    {
        Renderer.material = Material = Instantiate(Material);
    }

    public override void AnimateHighlight()
    {
        Material.color = Selected;
    }

    public override void AnimateInteraction()
    {
        Material.color = Activated;
    }

    public override void AnimateRecovery()
    {
        Material.color = Default;
    }

    public override void AnimateStopHighlight()
    {
        Material.color = Default;
    }
}
