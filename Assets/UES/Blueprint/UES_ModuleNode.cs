using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class UES_ModuleNode : Node
{
    [Input] public UES_BaseModule addTriggerInput;
    [Input] public UES_BaseModule addPowerInput;

    [Output] public UES_BaseModule setTriggerOutput;
    [Output] public UES_BaseModule setPowerOutput;

    public UES_BaseModule[] TriggerInputs;
    public UES_BaseModule[] PowerInputs;

    [SerializeField]
    [HideInInspector]
    private UES_BaseModule owner;

    public void SetOwningModule(UES_BaseModule mod)
    {
        owner = mod;
    }

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "setTriggerOutput")
        {
            return GetInputValue<UES_BaseModule>("addTriggerInput");
        }
        else if (port.fieldName == "setPowerOutput")
        {
            return GetInputValue<UES_BaseModule>("addPowerInput");
        }

        return null;
    }

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        if (from.fieldName == "setTriggerOutput" && to.fieldName == "addTriggerInput")
        {
            Debug.Log("Setting Trigger output");
        }
        else if (from.fieldName == "setPowerOutput" && to.fieldName == "addPowerInput")
        {
            Debug.Log("Setting Power output.");
        }
        else
            from.Disconnect(to);
    }

    public override void OnRemoveConnection(NodePort port)
    {
        base.OnRemoveConnection(port);
    }
}
