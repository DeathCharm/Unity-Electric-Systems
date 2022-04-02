using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// IOList for UES_BaseModules' Trigger and Power connections.
/// </summary>
[Serializable]
    public class UES_IOList:ARX_IOList<UES_BaseModule>
    {

    public enum ListType { Trigger, Power }

    [SerializeField]
    [HideInInspector]
    public ListType _eListType;

    
    public UES_IOList(UES_BaseModule module, ListType listType) : base(module) {
        _eListType = listType;
    }
    protected override List<UES_BaseModule> GetInputList(UES_BaseModule other)
    {
        switch (_eListType)
        {
            case ListType.Power:
                Debug.Log("Returning Power input list of " + other.name);
                return other._GetPowerInputList;
            case ListType.Trigger:
                Debug.Log("Returning Trigger input list of " + other.name);
                return other._GetTriggerInputList;
            default:
                return null;
        }
    }

    protected override List<UES_BaseModule> GetOutputList(UES_BaseModule other)
    {
        switch (_eListType)
        {
            case ListType.Power:
                Debug.Log("Returning Power output list of " + other.name);
                return other._GetPowerOutputList;
            case ListType.Trigger:
                Debug.Log("Returning Trigger output list of " + other.name);
                return other._GetTriggerOutputList;
            default:
                return null;
        }
    }

    public override UES_BaseModule GetNullModule()
    {
        return null;
    }

    protected override bool IsEqual(UES_BaseModule one, UES_BaseModule two)
    {
        return one == two;
    }

}

