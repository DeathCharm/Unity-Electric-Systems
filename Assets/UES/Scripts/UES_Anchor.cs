using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Anchor : UES_BaseModule
{

    Vector3 delta, lastPosition;
    public override void OnPowered()
    {
        base.OnPowered();
        foreach (UES_BaseModule mod in uesModules.GetTriggerOutputs)
        {
            mod.gameObject.transform.Translate(delta);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        delta = lastPosition - transform.position;

        lastPosition = transform.position;
    }

}
