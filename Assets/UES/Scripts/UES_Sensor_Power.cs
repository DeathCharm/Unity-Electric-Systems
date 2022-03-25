using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Sensor_Power : UES_BaseModule
{

    public List<UES_BaseModule> observedModules = new List<UES_BaseModule>();
    public override void OnPowered()
    {
        base.OnPowered();

        bool bAllPowerInputsActive = true;
        foreach (UES_BaseModule power in observedModules)
        {
            if (power.mb_isPowered == false)
                bAllPowerInputsActive = false;
        }

        mb_isUESModuleActive = bAllPowerInputsActive;
    }
}
