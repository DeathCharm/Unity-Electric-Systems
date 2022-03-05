using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Converter : UES_BaseModule
{

    public enum TriggerTiming {OnPowerGained, OnPowerLost, Both, WhenPowered }
    public TriggerTiming etriggerTiming = TriggerTiming.OnPowerGained;


    public override void OnPowerStart(UES_Signal signal)
    {
        if (etriggerTiming == TriggerTiming.Both || etriggerTiming == TriggerTiming.OnPowerGained)
            SendTrigger(signal);
    }

    public override void OnPowerLost()
    {
        if (etriggerTiming == TriggerTiming.Both || etriggerTiming == TriggerTiming.OnPowerLost)
            SendTrigger(signal);
    }

    public override void OnPowered()
    {
        if (etriggerTiming == TriggerTiming.WhenPowered)
            SendTrigger(signal);
    }
}
