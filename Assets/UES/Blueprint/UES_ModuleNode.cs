using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class UES_ModuleNode : Node
{
    [Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None)] public UES_BaseModule addTriggerInput;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None)] public UES_BaseModule addPowerInput;

    [Output] public UES_BaseModule setTriggerOutput;
    [Output] public UES_BaseModule setPowerOutput;


    [SerializeField]
    public UES_BaseModule owner;

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
        UES_BaseModule modFrom = ((UES_ModuleNode)from.node).owner;
        UES_BaseModule modTo = ((UES_ModuleNode)to.node).owner;

        
        if(owner == modFrom)
        if (from.fieldName == "setTriggerOutput" && to.fieldName == "addTriggerInput")
        {
            Debug.Log("Setting Trigger from " + modFrom.name + " to " + modTo.name);
            modTo.AddTriggerInput(modFrom);

        }
        else if (from.fieldName == "setPowerOutput" && to.fieldName == "addPowerInput")
        {
            Debug.Log("Setting Power from " + modFrom.name + " to " + modTo.name);
                modTo.AddPowerInput(modFrom);
        }
        else
            from.Disconnect(to);
    }

    public override void OnRemoveConnection(NodePort port, NodePort that)
    {
        UES_BaseModule portModule = ((UES_ModuleNode)port.node).owner;
        UES_BaseModule otherModule = ((UES_ModuleNode)that.node).owner;

        Debug.Log("Sensing detatchment of " + portModule.name + " from " + otherModule.name);

        if (port.fieldName == "addTriggerInput")
        {
            Debug.Log("Disconnecting " + name + "'s Trigger from " + otherModule.name);
            otherModule.RemoveTriggerOutput(owner);
        }
        else if (port.fieldName == "addPowerInput")
        {
            Debug.Log("Disconnecting " + name + "'s Power from " + otherModule.name);
            otherModule.RemovePowerOutput(owner);
        }

        base.OnRemoveConnection(port, that);
    }
}
