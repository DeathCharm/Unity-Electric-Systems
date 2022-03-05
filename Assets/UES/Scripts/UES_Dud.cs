using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Dud : UES_BaseModule
{
    public override void SendPowerToModules(UES_Signal signal)
    {
        if(isUESModuleActive)
        base.SendPowerToModules(signal);
    }

    public override void SendTrigger(UES_Signal signal)
    {
        if (isUESModuleActive)
            base.SendTrigger(signal);
    }

    public override void OnTriggered(UES_Signal signal)
    {
        isUESModuleActive = !isUESModuleActive;
    }
}
