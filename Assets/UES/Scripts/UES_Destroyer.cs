using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Destroyer : UES_BaseModule
{

    public bool targetPlayer = false;

    public override void OnTriggered(UES_Signal signal)
    {
        base.OnTriggered(signal);
        foreach (UES_BaseModule obj in uesModules.GetTriggerOutputs)
        {
            obj.OnDemolished();
        }

        if (targetPlayer)
        {
            UES.KillPlayer("a Destroyer");
        }
    }
}
