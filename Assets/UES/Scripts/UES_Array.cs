using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Array : UES_BaseModule
{
    public GameObject indicatorRoot;
    public int targetIndex = 0;

    public enum ArrayTarget { Trigger, Power, Both }
    public ArrayTarget arrayTarget = ArrayTarget.Power;

    UES_Light[] lights;

    public UES_Light[] GetLights
    {
        get
        {
            if (lights == null)
                lights = indicatorRoot.GetComponentsInChildren<UES_Light>(true);
            return lights;
        }
    }

    public int GetTargetLength
    {
        get
        {
            if (arrayTarget == ArrayTarget.Power)
                return uesModules.GetPowerOutputs.Count;
            else if (arrayTarget == ArrayTarget.Trigger)
                return uesModules.GetTriggerOutputs.Count;
            else
            {
                return uesModules.GetPowerOutputs.Count < uesModules.GetTriggerOutputs.Count ? uesModules.GetPowerOutputs.Count : uesModules.GetTriggerOutputs.Count;
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
            UES_Light o = GetLights[i];
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

        foreach (UES_BaseModule powerOutput in uesModules.GetPowerOutputs)
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

                if (targetIndex >= uesModules.GetTriggerOutputs.Count || targetIndex >= GetLights.Length)
                {
                    targetIndex = 0;
                }
            }
            else if (arrayTarget == ArrayTarget.Power)
            {
                if (targetIndex >= uesModules.GetPowerOutputs.Count || targetIndex >= GetLights.Length)
                {
                    targetIndex = 0;
                }
            }
            else
            {
                targetIndex++;

                if (targetIndex >= uesModules.GetTriggerOutputs.Count || targetIndex >= GetLights.Length || targetIndex >= uesModules.GetPowerOutputs.Count)
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
        List<UES_BaseModule> triggers = uesModules.GetTriggerOutputs;
        if (index >= triggers.Count || index < 0)
            return null;

        return uesModules.GetTriggerOutputs[index];
    }

    public UES_BaseModule GetPowerModule(int index)
    {
        List<UES_BaseModule> triggers = uesModules.GetPowerOutputs;
        if (index >= triggers.Count || index < 0)
            return null;

        return uesModules.GetPowerOutputs[index];
    }

    public override void SendTrigger(UES_Signal signal)
    {
        UES_BaseModule mod = GetTriggerModule(targetIndex);
        if (mod != null)
            mod.OnTriggered(signal);
    }

}
