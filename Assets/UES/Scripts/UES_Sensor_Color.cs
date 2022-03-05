using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Sensor_Color : UES_BaseModule
{
    public Color targetColor = Color.green;
    public override void OnPowered()
    {
        base.OnPowered(); 
        if (UES.playerColor == targetColor)
        {
            isUESModuleActive = true;
        }
        else
            isUESModuleActive = false;
    }

}
