using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Indicator : MonoBehaviour
{
    public UES_IndicatorLight sensorLight;

    ARX.UnityTimer timer = new ARX.UnityTimer();
    bool bIsTicking = false;
    public float touchLightTime = 0.2F;
    public void ActivateIndicator()
    {
        timer.Start(touchLightTime);
        bIsTicking = true;
        sensorLight.On();
    }

    public void FixedUpdate()
    {
        if (bIsTicking)
        {
            timer.Tick();
            if (timer.IsFinished)
            {
                bIsTicking = false;
                sensorLight.Off();
            }
        }
    }

}
