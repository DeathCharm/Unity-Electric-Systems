using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_BasicMiniComponents : MonoBehaviour
{
    public UES_Light powerLight;
    public GameObject powerOutput, triggerOutput, powerInput, triggerInput;

    GameObject Clone(GameObject obj)
    {
        GameObject o = Instantiate(obj);
        o.name = obj.name;
        return o;
    }

    public void ApplyMiniComponents(UES_BaseModule mod)
    {

        GameObject bufPowerLight, bufPowerOutput, bufPowerInput, bufTriggerOutput, bufTriggerInput;
        bufPowerLight = Clone(powerLight.gameObject);
        bufPowerOutput = Clone(powerOutput);
        bufTriggerOutput = Clone(triggerOutput);
        bufPowerInput = Clone(powerInput);
        bufTriggerInput = Clone(triggerInput);


        bufPowerOutput.transform.SetParent(mod.transform);
        bufTriggerOutput.transform.SetParent(mod.transform);
        bufPowerInput.transform.SetParent(mod.transform);
        bufTriggerInput.transform.SetParent(mod.transform);
        bufPowerLight.transform.SetParent(mod.transform);

        mod.mo_powerOutputModel = bufPowerOutput.gameObject;
        mod.mo_powerInputModel = bufPowerInput.gameObject;
        mod.mo_triggerOutputModel = bufTriggerOutput.gameObject;
        mod.mo_triggerInputModel = bufTriggerInput.gameObject;
        mod.mo_powerLight = bufPowerLight.GetComponent<UES_Light>();

        bufPowerLight.transform.localPosition = Vector3.zero;
        bufPowerOutput.transform.localPosition = Vector3.zero;
        bufTriggerOutput.transform.localPosition = Vector3.zero;
        bufPowerInput.transform.localPosition = Vector3.zero;
        bufTriggerInput.transform.localPosition = Vector3.zero;
    }
}
