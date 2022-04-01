using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class UES_Battery : UES_BaseModule
{
    public override void OnStart()
    {
        base.OnStart();
        foreach (UES_BaseModule mod in GetPowerOutputs)
        {
            if(mod != null)
            mod.OnPowerStart(signal);
        }
    }

    public override void OnTriggered(UES_Signal signal)
    {
        Debug.Log("Triggered battery.");
        base.OnTriggered(signal);
        mb_isUESModuleActive = !mb_isUESModuleActive;
    }

    /// <summary>
    /// If receiving power of any kind, is unpowered.
    /// Else, is powered.
    /// </summary>
    public override void OnFixedUpdate()
    {
        if (mb_isUESModuleActive == true && HasPowerInput() == false)
        {
            mb_isPowered = true; 
            if (mo_powerLight != null)
                mo_powerLight.On();
        }
        else
        {
            mb_isPowered = false; 
            if (mo_powerLight != null)
                mo_powerLight.Off();
        }

        if (mb_isPowered)
        {
            SendPowerToModules(signal);
        }

    }

    public override void ReceivePower(UES_Signal signal)
    {
        mb_isPowered = false;
    }

    public override void OnPowerLost()
    {
        mb_isPowered = true;
    }

    public override void Depower()
    {
        //Intentionally Blank

    }

}
