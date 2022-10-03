using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAColor : OAParticles
{
    public Renderer Renderer;
    public Material Material;
    public Color Default;
    public Color Selected;
    public Color Activated;

    protected override void Start()
    {
        base.Start();
        Renderer.material = Material = Instantiate(Material);
    }

    public override void AnimateHighlight()
    {
        base.AnimateHighlight();
        Material.color = Selected;
    }

    public override void AnimateInteraction()
    {
        base.AnimateInteraction();
        Material.color = Activated;
    }

    public override void AnimateRecovery()
    {
        base.AnimateRecovery();
        Material.color = Default;
    }

    public override void AnimateStopHighlight()
    {
        base.AnimateStopHighlight();
        Material.color = Default;
    }
}
