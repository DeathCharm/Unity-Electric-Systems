using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Sensor_Power : UES_BaseModule
{
    public override void OnPowered()
    {
        base.OnPowered();

        bool bAllPowerInputsActive = true;
        foreach (UES_BaseModule power in GetTriggerOutputs)
        {
            if (power.isPowered == false)
                bAllPowerInputsActive = false;
        }

        isUESModuleActive = bAllPowerInputsActive;
    }
}
