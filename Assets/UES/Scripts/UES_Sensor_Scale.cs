using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Sensor_Scale : UES_BaseModule
{
    List<GameObject> touchingObjects = new List<GameObject>();
    public float targetWeight = 1;
    public float currentWdight = 0;

    public UES_WaypointPositioner leftTrack, rightTrack;
    public UES_IndicatorLight indicator;

    bool bIsWeightedDown = false;

    float GetTotalWeight
    {
        get
        {
            float f = 0;
            foreach (GameObject obj in touchingObjects)
            {
                Rigidbody r = obj.GetComponent<Rigidbody>();
                if (r == null)
                    continue;
                f += r.mass;
            }
            return f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        touchingObjects.Add(other.gameObject);
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        touchingObjects.Clear();
    }

    private void OnTriggerExit(Collider other)
    {
        touchingObjects.Remove(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        touchingObjects.Add(other.gameObject);
    }

    public override void OnInactive()
    {
        //indicator.Off();
    }

    public override void OnUpdate()
    {
        currentWdight = GetTotalWeight;
        base.OnUpdate();
        if (currentWdight >= targetWeight)
        {
            //isUESModuleActive = true;
            bIsWeightedDown = true;
            indicator.On();
        }
        else
        {
            //isUESModuleActive = false;
            bIsWeightedDown = false;
            indicator.Off();
        }
    }

    public override void OnPowered()
    {
        base.OnPowered();
        float weightFactor = GetTotalWeight / targetWeight;
        leftTrack.percent = weightFactor;
        rightTrack.percent = weightFactor;
    }

    public override void SendPowerToModules(UES_Signal signal)
    {
        if (bIsWeightedDown == false)
            return;
        base.SendPowerToModules(signal);
    }
}
