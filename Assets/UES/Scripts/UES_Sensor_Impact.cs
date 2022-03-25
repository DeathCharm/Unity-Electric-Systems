using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class UES_Sensor_Impact : UES_BaseModule
{
    public float impactThreshhold = 1.0F;
    public UES_Indicator indicator;
    public UES_WaypointPositioner waypointPositioner;


    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Space))
            waypointPositioner.PlaceDisc(5);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (mb_isPowered == false)
            return;

        float impact = collision.relativeVelocity.magnitude;
        float impactFactor= impact / impactThreshhold;


        waypointPositioner.PlaceDisc(impactFactor);

        Debug.Log(name + " had an impact of " + impact);
        if (impact >= impactThreshhold)
        {
            SendTrigger(signal);
            indicator.ActivateIndicator();
        }
    }

}
