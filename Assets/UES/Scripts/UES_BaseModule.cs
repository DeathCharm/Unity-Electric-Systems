using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

[Serializable]
[ExecuteInEditMode]
public class UES_BaseModule : MonoBehaviour
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

    #region Components
    public UES_BaseModule_Wires wires;
    public UES_BaseModule_AttachedModules uesModules;
    public UES_BaseModule_CompModels models;
    #endregion

    #region Unity Functions

    private void OnValidate()
    {
        wires.CreateWires();
    }

    void CreateCompositionObjects()
    {
        if (wires == null)
            wires = new UES_BaseModule_Wires(this);
        if (uesModules == null)
            uesModules = new UES_BaseModule_AttachedModules(this);
        if (models == null)
            models = new UES_BaseModule_CompModels(this);
    }

    private void Start()
    {
        CreateCompositionObjects();

        OnStart();
    }

    public void Update()
    {

        CreateCompositionObjects();
        AddMiniComponents();
        OnUpdate();

        if (Application.isPlaying == false)
        {
            FixedUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (models.mo_powerLight != null)
            if (mb_isPowered)
            {
                models.mo_powerLight.On();
            }
            else
                models.mo_powerLight.Off();

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

    public void AddMiniComponents()
    {
        if (models.mo_addAllModels != null)
        {
            models.mo_addAllModels.ApplyMiniComponents(this);
            models.mo_addAllModels = null;
        }

        uesModules.AddMiniComponents();

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

    public void Add(UES_BaseModule module, List<UES_BaseModule> list)
    {
        list.Add(module);
    }

    public void Remove(UES_BaseModule module, List<UES_BaseModule> list)
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
        if (uesModules.GetTriggerOutputs.Count > 0)
        {
            //Debug.Log(name + " is sending power to its " + PowerOutputs.Count + " children.");
        }
        //For each power outupt, activate their Receive Power function
        foreach (UES_BaseModule module in uesModules.GetTriggerOutputs)
        {
            if (module != null && ModuleToReceiveTrigger(module))
            {
                Debug.Log(name + " is sending trigger to " + module.name);
                module.OnTriggered(signal);
                if (wires.wireDictionary.ContainsKey(module))
                    wires.wireDictionary[module].OnPowerUp();
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
        foreach (UES_BaseModule mod in uesModules.GetPowerInputs)
            if (mod != null && mod.mb_isPowered)
                return true;
        return false;

    }

    public virtual void SendPowerToModules(UES_Signal signal)
    {
        Debug.Log(name + " On Send Power");
        
        if (uesModules.GetPowerOutputs.Count > 0)
        {
            //Debug.Log(name + " is sending power to its " + PowerOutputs.Count + " children.");
        }
        //For each power outupt, activate their Receive Power function
        foreach (UES_BaseModule module in uesModules.GetPowerOutputs)
        {
            if (module != null && ModuleToReceivePower(module))
            {
                //Debug.Log(name + " is sending power to " + module.name);
                module.ReceivePower(signal);

                if (wires.wireDictionary.ContainsKey(module))
                    wires.wireDictionary[module].OnPowerUp();
            }
        }
    }
    #endregion

    #region Draw

    public virtual void DrawUESGizmo()
    {

    }

    public void OnDrawGizmos()
    {
        //Draw Trigger outputs
        foreach (UES_BaseModule mod in uesModules.GetTriggerOutputs)
        {
            if (mod == null)
                continue;

            if (mod.models.mo_triggerInputModel != null && models.mo_triggerOutputModel != null)
            {
                DrawGizmoLine(models.mo_triggerOutputModel, mod.models.mo_triggerInputModel, GetActiveColor);
            }
            else
            {
                Debug.LogError("Missing trigger input or output model for " + mod.name + " or " + name);
            }
        }

        foreach (UES_BaseModule mod in uesModules.GetPowerOutputs)
        {
            if (mod == null)
            {
                continue;
            }

            if (mod.models.mo_powerInputModel != null && models.mo_powerOutputModel != null)
            {
                DrawGizmoLine(models.mo_powerOutputModel, mod.models.mo_powerInputModel, GetActiveColor);
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

[Serializable]
public class UES_BaseModule_Wires : ScriptableObject
{
    [SerializeField]
    private UES_BaseModule script;
    public UES_BaseModule_Wires(UES_BaseModule owner)
    {
        script = owner;
    }

    [HideInInspector]
    public bool mb_createWires = false;

    [SerializeField]
    GameObject mo_wireRoot;

    [HideInInspector]
    public UES_WireRope[] moa_wires = new UES_WireRope[0];

    #region Wires
    [SerializeField]
    public Dictionary<UES_BaseModule, UES_WireRope> wireDictionary = new Dictionary<UES_BaseModule, UES_WireRope>();

    /// <summary>
    /// Returns the game object used as the root of this object's connected wire models
    /// </summary>
    public GameObject GetWireRoot
    {
        get
        {
            if (mo_wireRoot == null)
            {
                mo_wireRoot = new GameObject();
                mo_wireRoot.name = "Wire Root";
                mo_wireRoot.transform.SetParent(script.transform);
                mo_wireRoot.transform.localPosition = Vector3.zero;
            }

            return mo_wireRoot;
        }
    }


    /// <summary>
    /// For each wire rope in children, check to see if it points at an output.
    /// If the wire points at no output, delete the wire
    /// </summary>
    public void CheckWires()
    {
        UES_WireAnchor[] childRopes = script.GetComponentsInChildren<UES_WireAnchor>(true);
        for (int i = 0; i < childRopes.Length; i++)
        {
            bool bPointsToNoAnchor = true;
            foreach (UES_BaseModule mod in script.uesModules.GetPowerOutputs)
            {
                if (childRopes[i].anchorTarget == mod)
                {
                    bPointsToNoAnchor = false;
                    break;
                }
            }
            foreach (UES_BaseModule mod in script.uesModules.GetTriggerOutputs)
            {
                if (childRopes[i].anchorTarget == mod)
                {
                    bPointsToNoAnchor = false;
                    break;
                }
            }

            //if (bPointsToNoAnchor == true)
            //{
            //    childRopes[i].gameObject.AddComponent<DestroyMe>().enableDestruction = true;
            //    continue;
            //}
        }
    }

    /// <summary>
    /// If one of this object's child Wires already has the target mod as an anchor target,
    ///return true.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    public bool HasWireInChildren(UES_BaseModule mod)
    {

        UES_WireAnchor[] anchors = script.GetComponentsInChildren<UES_WireAnchor>(true);
        if (anchors.Length == 0)
            return false;
        foreach (UES_WireAnchor anchor in anchors)
        {
            if (anchor.anchorTarget == mod.gameObject)
            {
                Debug.Log("Anchor" + anchor.name + " already has wire pointing at " + mod.name + ". No wire will be created.");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Create a Wire prefab, set its anchor target to the given mod and put
    /// the created wire into a root gameobject
    /// </summary>
    /// <param name="mod"></param>
    public void CreateNewWire(UES_BaseModule mod)
    {
        Debug.Log("Instantiating new wire");

        GameObject newWire = GameObject.Instantiate(Resources.Load("Wire")) as GameObject;

        UES_WireAnchor anchor = newWire.GetComponent<UES_WireAnchor>();
        UES_WireRope wireRope = newWire.GetComponent<UES_WireRope>();

        if (anchor == null)
            Debug.LogError("Anchor is null");
        if (wireRope == null)
            Debug.LogError("Wire Rope is null");
        anchor.anchorTarget = mod.gameObject;
        anchor.enabled = true;
        newWire.transform.SetParent(GetWireRoot.transform);
        newWire.transform.localPosition = Vector3.zero;
        wireRope.mo_endObject = mod.gameObject;

        wireDictionary[mod] = wireRope;
    }

    /// <summary>
    ///For each module in the Trigger output and Power output list
    ///create a wire leading to it, activate its wire anchor and set this
    ///object as its anchor target
    /// </summary>
    public void CreateWires()
    {
        wireDictionary.Clear();
        Debug.Log(script.name + " is Creating Wires.");

        foreach (UES_BaseModule mod in script.uesModules.GetPowerOutputs)
        {
            Debug.Log(mod.name + " checking " + mod.name + "'s wires.");
            if (HasWireInChildren(mod))
            {
                Debug.Log(mod.name + " already has a wire from " + script.name + " pointing at it.");
                continue;
            }
            else
                CreateNewWire(mod);
        }
        foreach (UES_BaseModule mod in script.uesModules.GetTriggerOutputs)
        {
            Debug.Log(mod.name + " checking " + mod.name + "'s wires.");
            if (HasWireInChildren(mod))
            {
                Debug.Log(mod.name + " already has a wire from " + script.name + " pointing at it.");
                continue;
            }
            else
                CreateNewWire(mod);
        }
    }

    #endregion
}


    [Serializable]
public class UES_BaseModule_CompModels : ScriptableObject
{

    [SerializeField]
    private UES_BaseModule script;
    public UES_BaseModule_CompModels(UES_BaseModule owner)
    {
        script = owner;
    }

    public UES_BasicMiniComponents mo_addAllModels;

    //[Header("Models")]
    [HideInInspector]
    public GameObject mo_powerInputModel;
    [HideInInspector]
    public GameObject mo_triggerOutputModel;
    [HideInInspector]
    public GameObject mo_powerOutputModel;
    [HideInInspector]
    public GameObject mo_triggerInputModel;
    [HideInInspector]
    public UES_Light mo_powerLight;

}


[Serializable]
public class UES_BaseModule_AttachedModules : ScriptableObject{

    [SerializeField]
    private UES_BaseModule script;
    public UES_BaseModule_AttachedModules(UES_BaseModule owner)
    {
        script = owner;
    }

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


    #region UES Modules

    [Header("UES Modules")]

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
    #endregion

    #region Add/Remove
    public virtual void AddTriggerOutput(UES_BaseModule module)
    {
        if (module == null || module == script)
        {
            InvalidModule();
            return;
        }

        Debug.Log(script.name + " Adding Trigger Output.");
        if (!GetTriggerOutputs.Contains(module))
            GetTriggerOutputs.Add(module);


        script.wires.CreateWires();
    }

    public virtual void AddTriggerInput(UES_BaseModule module)
    {
        if (module == null || module == script)
        {
            InvalidModule();
            return;
        }
        Debug.Log(script.name + " Adding Trigger Input");
        if (!module.uesModules.GetTriggerOutputs.Contains(module))
        {
            module.uesModules.GetTriggerOutputs.Add(module);
            module.wires.CreateWires();
        }
    }

    public virtual void RemoveTriggerOutput(UES_BaseModule module)
    {
        Debug.Log(script.name + " Removing Trigger Output");
        GetTriggerOutputs.Remove(module);
    }
    public virtual void AddPowerOutput(UES_BaseModule module)
    {
        if (module == null || module == script)
        {
            InvalidModule();
            return;
        }
        Debug.Log(script.name + " Adding Power Output");
        if (!GetPowerOutputs.Contains(module))
            GetPowerOutputs.Add(module);

        if (!module.uesModules.PowerInputs.Contains(module))
            module.uesModules.PowerInputs.Add(script);

        script.wires.CreateWires();
    }
    public virtual void RemovePowerOutput(UES_BaseModule module)
    {

        Debug.Log(script.name + " Removing Power Output");
        GetPowerOutputs.Remove(module);
        module.uesModules.PowerInputs.Remove(script);
    }
    public virtual void AddPowerInput(UES_BaseModule module)
    {
        if (module == null || module == script)
        {
            InvalidModule();
            return;
        }
        Debug.Log(script.name + " Adding Power Input");
        if (!PowerInputs.Contains(module))
        {
            PowerInputs.Add(module);
        }

        if (!module.uesModules.GetPowerOutputs.Contains(script))
            module.uesModules.GetPowerOutputs.Add(script);

        module.wires.CreateWires();
    }
    public virtual void RemovePowerInput(UES_BaseModule module)
    {
        Debug.Log(script.name + " Removing Power Input");
        PowerInputs.Remove(module);
        module.uesModules.GetPowerOutputs.Remove(script);
    }



    public void RemoveModule(UES_BaseModule mod)
    {
        RemovePowerInput(mod);
        RemovePowerOutput(mod);
        RemoveTriggerOutput(mod);
    }
    #endregion

    public void InvalidModule()
    {
        Debug.LogError("Null object given as module.");
    }

    public void AddMiniComponents()
    {

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
}

