using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Array : UES_BaseModule
{
    public GameObject indicatorRoot;
    public int targetIndex = 0;

    public enum ArrayTarget { Trigger, Power, Both }
    public ArrayTarget arrayTarget = ArrayTarget.Power;

    UES_IndicatorLight[] lights;

    public UES_IndicatorLight[] GetLights
    {
        get
        {
            if (lights == null)
                lights = indicatorRoot.GetComponentsInChildren<UES_IndicatorLight>(true);
            return lights;
        }
    }

    public int GetTargetLength
    {
        get
        {
            if (arrayTarget == ArrayTarget.Power)
                return GetPowerOutputs.Length;
            else if (arrayTarget == ArrayTarget.Trigger)
                return GetTriggerOutputs.Length;
            else
            {
                return GetPowerOutputs.Length < GetTriggerOutputs.Length ? GetPowerOutputs.Length : GetTriggerOutputs.Length;
            }
        }
    }

    public override void OnUpdate()
    {
        if (targetIndex < 0)
            targetIndex = 0;
        if (targetIndex > GetTargetLength -1)
            targetIndex = GetTargetLength -1;
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        for (int i = 0; i < GetLights.Length; i++)
        {
            UES_IndicatorLight o = GetLights[i];
            if (i == targetIndex)
            {

                o.gameObject.SetActive(true);
                o.On();
            }
            else
            {
                if (i >= GetTargetLength)
                {
                    o.gameObject.SetActive(false);
                }
                else
                    o.Off();
            }
        }
    }

    public override void ReceivePower(UES_Signal signal)
    {
        base.ReceivePower(signal);
        Debug.Log("Taking Power");
    }


    public override void SendPowerToModules(UES_Signal signal)
    {
        UES_BaseModule targetArrayMod = GetPowerModule(targetIndex);

        foreach (UES_BaseModule powerOutput in GetPowerOutputs)
        {
            if (powerOutput == null)
                continue;

            if (powerOutput == targetArrayMod)
            {
                if(targetArrayMod != null)
                    targetArrayMod.ReceivePower(signal);
            }
            else
                powerOutput.Depower();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signal"></param>
    public override void OnTriggered(UES_Signal signal)
    {
        if (signal.Contains("index"))
        {
            if (arrayTarget == ArrayTarget.Trigger)
            {
                targetIndex++;

                if (targetIndex >= GetTriggerOutputs.Length || targetIndex >= GetLights.Length)
                {
                    targetIndex = 0;
                }
            }
            else if (arrayTarget == ArrayTarget.Power)
            {
                if (targetIndex >= GetPowerOutputs.Length || targetIndex >= GetLights.Length)
                {
                    targetIndex = 0;
                }
            }
            else
            {
                targetIndex++;

                if (targetIndex >= GetTriggerOutputs.Length || targetIndex >= GetLights.Length || targetIndex >= GetPowerOutputs.Length)
                {
                    targetIndex = 0;
                }

            }
        }
        else
        {
            if (arrayTarget == ArrayTarget.Trigger || arrayTarget == ArrayTarget.Both)
            {
                SendTrigger(signal);
            }
        }
    }

    public override bool ModuleToReceivePower(UES_BaseModule mod)
    {
        if (arrayTarget == ArrayTarget.Power || arrayTarget == ArrayTarget.Both)
        {
            if (mod == GetPowerModule(targetIndex))
                return true;
            else
                return false;
        }
        return true;
    }

    public override bool ModuleToReceiveTrigger(UES_BaseModule mod)
    {
        if (arrayTarget == ArrayTarget.Trigger || arrayTarget == ArrayTarget.Both)
        {
            if (mod == GetTriggerModule(targetIndex))
                return true;
            else
                return false;
        }
        return true;
    }

    public UES_BaseModule GetTriggerModule(int index)
    {
        UES_BaseModule[] triggers = GetTriggerOutputs;
        if (index >= triggers.Length || index < 0)
            return null;

        return GetTriggerOutputs[index];
    }

    public UES_BaseModule GetPowerModule(int index)
    {
        UES_BaseModule[] triggers = GetPowerOutputs;
        if (index >= triggers.Length || index < 0)
            return null;

        return GetPowerOutputs[index];
    }

    public override void SendTrigger(UES_Signal signal)
    {
        UES_BaseModule mod = GetTriggerModule(targetIndex);
        if (mod != null)
            mod.OnTriggered(signal);
    }

}
