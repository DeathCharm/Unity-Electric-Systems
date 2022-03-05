using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Obstacle : UES_BaseModule
{
    public override void OnDemolished()
    {
        SendTrigger(signal);
        base.OnDemolished();
    }
}
