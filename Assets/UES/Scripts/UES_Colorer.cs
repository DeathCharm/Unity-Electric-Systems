using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Colorer : UES_BaseModule
{
    public MeshRenderer oRenderer;
    public Color color = Color.green;
    public override void OnTriggered(UES_Signal signal)
    {
        UES.ColorPlayer(color);
        base.OnTriggered(signal);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (oRenderer != null)
            oRenderer.sharedMaterial.color = color;
    }
}
