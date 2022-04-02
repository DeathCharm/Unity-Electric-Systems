using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;


//Connected UES Modules
public partial class UES_BaseModule : MonoBehaviour
{
    #region Attached Module Variables

    //[Header("Quick Adds")]
    [HideInInspector]
    public UES_BaseModule mo_addTriggerOut;
    [HideInInspector]
    public UES_BaseModule mo_addPowerOut;
    [HideInInspector]
    public UES_BaseModule mo_addPowerIn;

    //[Header("Quick Removes")]
    [HideInInspector]
    public UES_BaseModule mo_removeTriggerOut;
    [HideInInspector]
    public UES_BaseModule mo_removePowerOut;
    [HideInInspector]
    public UES_BaseModule mo_removePowerIn;

    #endregion


    #region UES Modules

    [Header("UES Modules")]

    [SerializeField]
    [Tooltip("UES Modules that will be triggered when this module is triggered")]
    private UES_IOList Triggers;
    public UES_IOList GetTriggerIO
    {
        get
        {
            if (Triggers == null) Triggers = new UES_IOList(this, UES_IOList.ListType.Trigger);

            Triggers._eListType = UES_IOList.ListType.Trigger;
            return Triggers;
        }
    }

    [SerializeField]
    [Tooltip("UES Modules that will be powered when this module is powered")]
    private UES_IOList Powers;
    public UES_IOList GetPowerIO
    {
        get
        {
            if (Powers == null) Powers = new UES_IOList(this, UES_IOList.ListType.Power);

            Powers._eListType = UES_IOList.ListType.Power;
            return Powers;
        }
    }

    public List<UES_BaseModule> _GetPowerInputList { get { return Powers.Inputs; } }
    public List<UES_BaseModule> _GetPowerOutputList { get { return Powers.Outputs; } }
    public List<UES_BaseModule> _GetTriggerInputList { get { return Triggers.Inputs; } }
    public List<UES_BaseModule> _GetTriggerOutputList { get { return Triggers.Outputs; } }

    public UES_BaseModule[] GetTriggerOutputs
    {
        get
        {
            return GetTriggerIO.GetOutputArray;
        }
    }

    public UES_BaseModule[] GetTriggerInputs
    {
        get
        {
            return GetTriggerIO.GetInputArray;
        }
    }

    public UES_BaseModule[] GetPowerOutputs
    {
        get
        {
            return GetPowerIO.GetOutputArray;
        }
    }

    public UES_BaseModule[] GetPowerInputs
    {
        get
        {
            return GetPowerIO.GetInputArray;
        }
    }
    #endregion


    #region Add/Remove

    public virtual void AddTriggerInput(UES_BaseModule inputSource)
    {
        GetTriggerIO.AddInput(inputSource);
        CreateWires();
    }

    public virtual void AddTriggerOutput(UES_BaseModule module)
    {
        GetTriggerIO.AddOutput(module);
        CreateWires();
    }

    public virtual void AddPowerInput(UES_BaseModule module)
    {
        Debug.Log("Adding Power module input " + module.name + " to " + name);
        GetPowerIO.AddInput(module);
        module.CreateWires();
    }
    public virtual void AddPowerOutput(UES_BaseModule module)
    {
        Debug.Log("Adding Power module output " + module.name + " to " + name);
        GetPowerIO.AddOutput(module);
        CreateWires();
    }
    public virtual void RemoveTriggerInput(UES_BaseModule module)
    {
        GetTriggerIO.RemoveInput(module);
    }

    public virtual void RemoveTriggerOutput(UES_BaseModule module)
    {
        GetTriggerIO.RemoveOutput(module);
    }

    public virtual void RemovePowerInput(UES_BaseModule module)
    {
        Debug.Log(name + " Removing Power Input");
        GetPowerIO.RemoveInput(module);
    }

    public virtual void RemovePowerOutput(UES_BaseModule module)
    {
        GetPowerIO.RemoveOutput(module);
    }
    public void RemoveModule(UES_BaseModule mod)
    {
        RemovePowerInput(mod);
        RemovePowerOutput(mod);
        RemoveTriggerOutput(mod);
        RemoveTriggerInput(mod);
    }
    #endregion
}

