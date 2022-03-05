using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Splitter : UES_BaseModule
{
    public override void OnTriggered(UES_Signal signal)
    {
        SendTrigger(signal);
    }
}
