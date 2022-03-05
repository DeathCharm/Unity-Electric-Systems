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
        isUESModuleActive = !isUESModuleActive;
    }

    /// <summary>
    /// If receiving power of any kind, is unpowered.
    /// Else, is powered.
    /// </summary>
    public override void OnFixedUpdate()
    {
        if (isUESModuleActive == true && HasPowerInput() == false)
        {
            isPowered = true; 
            if (mo_powerLight != null)
                mo_powerLight.On();
        }
        else
        {
            isPowered = false; 
            if (mo_powerLight != null)
                mo_powerLight.Off();
        }

        if (isPowered)
        {
            SendPowerToModules(signal);
        }

    }

    public override void ReceivePower(UES_Signal signal)
    {
        isPowered = false;
    }

    public override void OnPowerLost()
    {
        isPowered = true;
    }

    public override void Depower()
    {
        //Intentionally Blank

    }

}
