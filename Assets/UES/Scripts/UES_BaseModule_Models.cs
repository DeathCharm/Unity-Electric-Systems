using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

//Models
public partial class UES_BaseModule:MonoBehaviour
{

    #region Models

    [SerializeField]
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
    public UES_IndicatorLight mo_powerLight;
    #endregion
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

