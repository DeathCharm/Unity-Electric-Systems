using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Timer : UES_BaseModule
{
    public bool timerStopped = true;
    ARX.UnityTimer timer = new ARX.UnityTimer();

    public bool loop = false;
    public float timeToElapse = 3.0F;
    public TMPro.TMP_Text display;

    public enum TriggerRepsonse { Restart, StopOrResume, End }
    public TriggerRepsonse triggerResponse = TriggerRepsonse.StopOrResume;

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (display != null)
        {
            float fTime = (timeToElapse - timer.mnf_timeElapsed);
            string strTime = fTime.ToString("0.00");

            Color c = Color.green;

            if (fTime == timeToElapse)
                c = Color.yellow;
            else if (fTime <= 0)
                c = Color.red;

            display.text = strTime;
            display.color = c;
        }
    }

    public override void OnPowered()
    {
        base.OnPowered();
        if (timerStopped == false)
        {
            timer.Tick();
            if (timer.IsFinished)
            {
                SendTrigger(signal);
                timer.Reset();

                if (loop == false)
                {
                    timerStopped = true;
                }

            }
        }
    }

    public override void OnTriggered(UES_Signal signal)
    {
        switch (triggerResponse)
        {
            case TriggerRepsonse.Restart:
                Debug.Log("Restarting Timer");
                timer.Start(timeToElapse);
                break;
            case TriggerRepsonse.StopOrResume:
                Debug.Log("Stop/Resuming Timer");
                timerStopped = !timerStopped;
                if (timer.IsFinished)
                    timer.Start(timeToElapse);
                break;
            case TriggerRepsonse.End:

                Debug.Log("Ending Timer");
                timer.Reset();
                timerStopped = true;
                SendTrigger(signal);
                break;
        }
        base.OnTriggered(signal);
    }
}
