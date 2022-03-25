using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Colorer : UES_BaseModule
{
    public ARX_Script_IndividualColor oRenderer;
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
            oRenderer.mo_color = color;
    }
}
