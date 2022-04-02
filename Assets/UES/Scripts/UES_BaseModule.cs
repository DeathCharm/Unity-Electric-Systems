using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

[Serializable]
[ExecuteInEditMode]
public partial class UES_BaseModule : MonoBehaviour
{
    #region Variables

    [Header("Variables")]
    public UES_Signal signal;
    public string notes = "Notes.";

    public bool mb_isUESModuleActive = true;

    [HideInInspector]
    public bool mb_isPowered = false;
    [HideInInspector]
    public bool mb_isPoweredThisFrame = false;
    [HideInInspector]
    public bool mb_isTriggeredThisFrame = false;

    #endregion

    #region Unity Functions
    private void OnDestroy()
    {
        GetPowerIO.Clear();
        GetTriggerIO.Clear();

    }

    private void OnValidate()
    {
        CreateWires();
    }

    private void Awake()
    {
        GetTriggerIO.SetOwner(this);
        GetPowerIO.SetOwner(this);
    }


    private void Start()
    {
        OnStart();
    }

    public void Update()
    {
        Validate();
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
            if (mb_isPowered)
            {
                mo_powerLight.On();
            }
            else
                mo_powerLight.Off();

        if (mb_isUESModuleActive == false)
        {
            OnInactive();
            mb_isPoweredThisFrame = false;
            return;
        }

        //Debug.Log("Fixed Updating " + name);
        //If powered this frame
        if (mb_isPowered || mb_isPoweredThisFrame)
        {
            //If power stopped this frame
            if (mb_isPoweredThisFrame == false)
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

        mb_isPoweredThisFrame = false;
        OnFixedUpdate();
    }

    #endregion

    #region Helper

    public void InvalidModule()
    {
        Debug.LogError("Null object given as module to " + name);
    }

    void Validate()
    {
        GetPowerIO.Validate();
        GetTriggerIO.Validate();
    }

    public Color GetActiveColor
    {
        get
        {
            if (mb_isPowered)
                return Color.green;
            else
                return Color.red;
        }
    }

    #endregion

    #region Virtuals

    /// <summary>
    /// If the module was powered laft frame, but is not powered this frame
    /// </summary>
    public virtual void Depower()
    {
        mb_isPowered = false;
    }

    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }

    public virtual void OnPowered() {
        
    }
    public virtual void OnTriggered(UES_Signal signal) {

        Debug.Log(name + " was triggered.");
    }
    public virtual void OnUnpowered(UES_Signal signal) { }

    /// <summary>
    /// Occurs immediately after DePower
    /// </summary>
    public virtual void OnPowerLost() {
        foreach (UES_WireRope rope in GetComponentsInChildren<UES_WireRope>(true))
            rope.OnPowerDown();
    }
    public virtual void OnPowerStart(UES_Signal signal) 
    {
        
    
    }
    public virtual void OnReset(UES_Signal signal) { }

    public virtual void OnInactive() { }

    public virtual void OnStart() { }
    public virtual void OnDemolished() { }
    public virtual void SendTrigger(UES_Signal signal)
    {
        if (GetTriggerOutputs.Length > 0)
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
                if (wireDictionary.ContainsKey(module))
                    wireDictionary[module].OnPowerUp();
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
        if (mb_isPoweredThisFrame)
        {
            //Debug.Log(name + " is already powered. Returning.");
            return;
        }

        if (mb_isPowered == false)
        {
            Debug.Log(name + " is powering up.");
            OnPowerStart(signal);
            mb_isPowered = true;
        }

        mb_isPoweredThisFrame = true;
        SendPowerToModules(signal);
    }

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
        foreach (UES_BaseModule mod in GetPowerInputs)
            if (mod != null && mod.mb_isPowered)
                return true;
        return false;

    }

    public virtual void SendPowerToModules(UES_Signal signal)
    {
        //Debug.Log(name + " On Send Power");
        
        if (GetPowerOutputs.Length > 0)
        {
            //Debug.Log(name + " is sending power to its " + PowerOutputs.Count + " children.");
        }
        //For each power outupt, activate their Receive Power function
        foreach (UES_BaseModule module in GetPowerOutputs)
        {
            if (module != null && ModuleToReceivePower(module))
            {
                //Debug.Log(name + " is sending power to " + module.name);
                module.ReceivePower(signal);

                if (wireDictionary.ContainsKey(module))
                    wireDictionary[module].OnPowerUp();
            }
        }
    }
    #endregion

}
