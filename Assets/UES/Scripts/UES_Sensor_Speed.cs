using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Sensor_Speed : UES_BaseModule
{

    const float minMeterAngle = -45;
    const float maxMeterAngle = 230;

    public Transform meterPivot;
    public float targetSpeed = 4;

    float GetAngle(float factor)
    {
        if (factor > 1)
            factor = 1;
        if (factor < 0)
            factor = 0;

        float totalAngles = Mathf.Abs(minMeterAngle) + maxMeterAngle;
        totalAngles *= factor;

        return minMeterAngle + totalAngles;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Vector3 vec = meterPivot.localEulerAngles;
        float factor = UES.PlayerSpeed / targetSpeed;
        vec.y = GetAngle(factor);
        meterPivot.localEulerAngles = vec;
    }

    public override void SendPowerToModules(UES_Signal signal)
    {
        if (UES.PlayerSpeed >= targetSpeed)
        {
            base.SendPowerToModules(signal);
        }
        else
            return;
    }

    public override void SendTrigger(UES_Signal signal)
    {
        if (UES.PlayerSpeed >= targetSpeed)
        {
            base.SendTrigger(signal);
        }
        else
            return;
    }

    public override void OnTriggered(UES_Signal signal)
    {
        if (UES.PlayerSpeed >= targetSpeed)
        {
            base.SendTrigger(signal);
        }
        else
            return;
    }
}
