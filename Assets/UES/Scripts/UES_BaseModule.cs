using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[ExecuteInEditMode]
public class UES_BaseModule : MonoBehaviour
{
    public string notes = "Notes.";
    public bool isUESModuleActive = true;

    [Header("Models")]
    public GameObject mo_powerInputModel;
    public GameObject mo_triggerOutputModel;
    public GameObject mo_powerOutputModel;
    public GameObject mo_triggerInputModel;
    public UES_Light mo_powerLight;

    [Header("Quick Adds")]
    public UES_BasicMiniComponents mo_addAll;
    public UES_BaseModule mo_addTriggerOut;
    public UES_BaseModule mo_addPowerOut;
    public UES_BaseModule mo_addPowerIn;

    [Header("Quick Removes")]
    public UES_BaseModule mo_removeTriggerOut;
    public UES_BaseModule mo_removePowerOut;
    public UES_BaseModule mo_removePowerIn;

    [Header("Modules")]
    [SerializeField]
    [Tooltip("UES Modules that will be triggered when this module is triggered")]
    private List<UES_BaseModule> TriggerOutputs;
    public List<UES_BaseModule> GetTriggerOutputs
    {
        get
        {
            if (TriggerOutputs == null)
                TriggerOutputs = new List<UES_BaseModule>();
            return TriggerOutputs;
        }
    }
    [SerializeField]
    [Tooltip("UES Modules that will be powered when this module is powered")]
    private List<UES_BaseModule> PowerOutputs;
    public List<UES_BaseModule> GetPowerOutputs
    {
        get
        {
            if (PowerOutputs == null)
                PowerOutputs = new List<UES_BaseModule>();
            return PowerOutputs;
        }
    }
    [SerializeField]
    [Tooltip("UES Modules that this module will request power from.")]
    private List<UES_BaseModule> PowerInputs;
    public List<UES_BaseModule> GetPowerInputs
    {
        get
        {
            if (PowerInputs == null)
                PowerInputs = new List<UES_BaseModule>();
            return PowerInputs;
        }
    }

    [Header("Variables")]
    public bool isPowered = false;
    public bool isPoweredThisFrame = false;
    public bool isTriggeredThisFrame = false;

    public UES_Signal signal;

    #region Unity Functions

    private void Start()
    {
        OnStart();
    }

    public void Update()
    {
        AddMiniComponents();
        OnUpdate();

        if (Application.isPlaying == false)
        {
            FixedUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (mo_powerLight != null)
            if (isPowered)
            {
                mo_powerLight.On();
            }
            else
                mo_powerLight.Off();

        if (isUESModuleActive == false)
        {
            OnInactive();
            isPoweredThisFrame = false;
            return;
        }

        //Debug.Log("Fixed Updating " + name);
        //If powered this frame
        if (isPowered || isPoweredThisFrame)
        {
            //If power stopped this frame
            if (isPoweredThisFrame == false)
            {
                //Debug.Log(name + " is not powered this frame. Powering down.");
                Depower();
                OnPowerLost();

            }
            else
            {
                OnPowered();
            }
        }
        else
        {
            OnInactive();
        }

        isPoweredThisFrame = false;
        OnFixedUpdate();
    }

    #endregion

    #region Helper
    void InvalidModule()
    {
        Debug.LogError("Null object given as module.");
    }

    Color GetActiveColor
    {
        get
        {
            if (isPowered)
                return Color.green;
            else
                return Color.red;
        }
    }
    private void AddMiniComponents()
    {
        if (mo_addAll != null)
        {
            mo_addAll.ApplyMiniComponents(this);
            mo_addAll = null;
        }
        //Add
        if (mo_addTriggerOut != null)
        {
            AddTriggerOutput(mo_addTriggerOut);
            mo_addTriggerOut = null;
        }
        if (mo_addPowerOut != null)
        {
            AddPowerOutput(mo_addPowerOut);
            mo_addPowerOut = null;
        }
        if (mo_addPowerIn != null)
        {
            AddPowerInput(mo_addPowerIn);
            mo_addPowerIn = null;
        }

        //Remove
        if (mo_removeTriggerOut != null)
        {
            RemoveTriggerOutput(mo_removeTriggerOut);
            mo_removeTriggerOut = null;
        }
        if (mo_removePowerOut != null)
        {
            RemovePowerOutput(mo_removePowerOut);
            mo_removePowerOut = null;
        }
        if (mo_removePowerIn != null)
        {
            RemovePowerInput(mo_removePowerIn);
            mo_removePowerIn = null;
        }
    }

    void Add(UES_BaseModule module, List<UES_BaseModule> list)
    {
        list.Add(module);
    }

    void Remove(UES_BaseModule module, List<UES_BaseModule> list)
    {
        list.Remove(module);
    }
    #endregion

    #region Virtuals

    /// <summary>
    /// If the module was powered laft frame, but is not powered this frame
    /// </summary>
    public virtual void Depower()
    {
        isPowered = false;
    }

    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }

    public virtual void OnPowered() { }
    public virtual void OnTriggered(UES_Signal signal) {

        Debug.Log(name + " was triggered.");
    }
    public virtual void OnUnpowered(UES_Signal signal) { }

    /// <summary>
    /// Occurs immediately after DePower
    /// </summary>
    public virtual void OnPowerLost() { }
    public virtual void OnPowerStart(UES_Signal signal) { }
    public virtual void OnReset(UES_Signal signal) { }

    public virtual void OnInactive() { }

    public virtual void OnStart() { }
    public virtual void OnDemolished() { }
    public virtual void SendTrigger(UES_Signal signal)
    {
        if (GetTriggerOutputs.Count > 0)
        {
            //Debug.Log(name + " is sending power to its " + PowerOutputs.Count + " children.");
        }
        //For each power outupt, activate their Receive Power function
        foreach (UES_BaseModule module in GetTriggerOutputs)
        {
            if (module != null && ModuleToReceiveTrigger(module))
            {
                Debug.Log(name + " is sending trigger to " + module.name);
                module.OnTriggered(signal);
            }
        }
    }

    /// <summary>
    /// Called by a power granting module when this module receives power from it.
    /// </summary>
    /// <param name="signal"></param>
    public virtual void ReceivePower(UES_Signal signal)
    {
        //If already powered, return to prevent infinite loops
        if (isPoweredThisFrame)
        {
            //Debug.Log(name + " is already powered. Returning.");
            return;
        }

        if (isPowered == false)
        {
            Debug.Log(name + " is powering up.");
            OnPowerStart(signal);
            isPowered = true;
        }

        isPoweredThisFrame = true;
        SendPowerToModules(signal);
    }
    #endregion

    #region Add/Remove
    public virtual void AddTriggerOutput(UES_BaseModule module) {
        if (module == null || module == this)
        {
            InvalidModule();
            return;
        }
        if (!GetTriggerOutputs.Contains(module))
            GetTriggerOutputs.Add(module);
    }

    public virtual void AddTriggerInput(UES_BaseModule module)
    {
        if (module == null || module == this)
        {
            InvalidModule();
            return;
        }
        if (!module.GetTriggerOutputs.Contains(module))
            module.GetTriggerOutputs.Add(module);
    }

    public virtual void RemoveTriggerOutput(UES_BaseModule module) {
        GetTriggerOutputs.Remove(module);
    }
    public virtual void AddPowerOutput(UES_BaseModule module) {
        if (module == null || module == this)
        {
            InvalidModule();
            return;
        }
        if (!GetPowerOutputs.Contains(module))
            GetPowerOutputs.Add(module);

        if(!module.PowerInputs.Contains(module))
            module.PowerInputs.Add(this);
    }
    public virtual void RemovePowerOutput(UES_BaseModule module) {

        Debug.Log("Removing Power Output");
        GetPowerOutputs.Remove(module);
        module.PowerInputs.Remove(this);
    }
    public virtual void AddPowerInput(UES_BaseModule module)
    {
        Debug.Log("Adding Power Input");
        if (module == null || module == this)
        {
            InvalidModule();
            return;
        }
        if (!PowerInputs.Contains(module))
            PowerInputs.Add(module);

        if(!module.GetPowerOutputs.Contains(this))
            module.GetPowerOutputs.Add(this);
    }
    public virtual void RemovePowerInput(UES_BaseModule module) {
        Debug.Log("Removing Power Input");
        PowerInputs.Remove(module);
        module.GetPowerOutputs.Remove(this);
    }


    #endregion

    #region Basic Module Functions

    public virtual bool ModuleToReceivePower(UES_BaseModule mod)
    {
        return true;
    }

    public virtual bool ModuleToReceiveTrigger(UES_BaseModule mod)
    {
        return true;
    }

    public bool HasPowerInput()
    {
        foreach (UES_BaseModule mod in PowerInputs)
            if (mod != null && mod.isPowered)
                return true;
        return false;

    }


    public virtual void SendPowerToModules(UES_Signal signal)
    {
        if (GetPowerOutputs.Count > 0)
        {
            //Debug.Log(name + " is sending power to its " + PowerOutputs.Count + " children.");
        }
        //For each power outupt, activate their Receive Power function
        foreach (UES_BaseModule module in GetPowerOutputs)
        {
            if(module != null && ModuleToReceivePower(module))
            //Debug.Log(name + " is sending power to " + module.name);
            module.ReceivePower(signal);
        }
    }

    public void RemoveModule(UES_BaseModule mod)
    {
        RemovePowerInput(mod);
        RemovePowerOutput(mod);
        RemoveTriggerOutput(mod);
    }
    #endregion

    #region Draw

    public virtual void DrawUESGizmo()
    {

    }

    public void OnDrawGizmos()
    {
        //Draw Trigger outputs
        foreach (UES_BaseModule mod in GetTriggerOutputs)
        {
            if (mod == null)
                continue;

            if (mod.mo_triggerInputModel != null && mo_triggerOutputModel != null)
            {
                DrawGizmoLine(mo_triggerOutputModel, mod.mo_triggerInputModel, GetActiveColor);
            }
            else
            {
                Debug.LogError("Missing trigger input or output model for " + mod.name + " or " + name);
            }
        }

        foreach (UES_BaseModule mod in GetPowerOutputs)
        {
            if (mod == null)
            {
                continue;
            }

            if (mod.mo_powerInputModel != null && mo_powerOutputModel != null)
            {
                DrawGizmoLine(mo_powerOutputModel, mod.mo_powerInputModel, GetActiveColor);
            }
            else
            {
                //Debug.LogError("Missing power input or output model for " + mod.name + " or " + name);
            }

        }
    }


    void DrawGizmoLine(GameObject o1, GameObject o2, Color color)
    {
        Color cBuf = color;
        Gizmos.color = color;
        Gizmos.DrawLine(o2.transform.position, o1.transform.position);
        Gizmos.color = cBuf;
    }
    #endregion

}
