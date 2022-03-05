using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_PhysicsAction : UES_BaseModule
{
    public bool targetPlayer = true;
    public Rigidbody[] targets = new Rigidbody[0];
    public ForceMode forceMode = ForceMode.Impulse;
    public Vector3 force = new Vector3(0, 4, 0);

    public UES_Light realImpulse, realAccel, unrealImpulse, unrealAccel;

    public override void OnTriggered(UES_Signal signal)
    {
        base.OnTriggered(signal);

        foreach(Rigidbody r in targets)
            r.AddForce(force, forceMode);

        if (targetPlayer == true)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
                Rigidbody r = obj.GetComponent<Rigidbody>();
                if (r != null)
                    r.AddForce(force, forceMode);
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        realImpulse.Off();
        realAccel.Off();
        unrealImpulse.Off();
        unrealAccel.Off();
        switch (forceMode)
        {
            case ForceMode.Acceleration:
                unrealAccel.On();
                break;
            case ForceMode.Force:
                realAccel.On();
                break;
            case ForceMode.Impulse:
                realImpulse.On();
                break;
            case ForceMode.VelocityChange:
                unrealImpulse.On();
                break;
        }
    }

}
