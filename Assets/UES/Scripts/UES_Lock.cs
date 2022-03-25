using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Lock : UES_BaseModule
{
    public override void OnUpdate()
    {
        bool bAllPowerInputsActive = true;
        foreach (UES_BaseModule power in uesModules.GetPowerInputs)
        {
            if (power.mb_isUESModuleActive == false || power.mb_isPowered == false)
                bAllPowerInputsActive = false;
        }
        mb_isUESModuleActive = bAllPowerInputsActive;
        base.OnUpdate();
    }
}
