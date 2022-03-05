using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Lock : UES_BaseModule
{
    public override void OnUpdate()
    {
        bool bAllPowerInputsActive = true;
        foreach (UES_BaseModule power in GetPowerInputs)
        {
            if (power.isUESModuleActive == false || power.isPowered == false)
                bAllPowerInputsActive = false;
        }
        isUESModuleActive = bAllPowerInputsActive;
        base.OnUpdate();
    }
}
