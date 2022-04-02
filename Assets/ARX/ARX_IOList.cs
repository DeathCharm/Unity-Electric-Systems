using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Template class makes sure that two lists held by TOwner classes have matching members in two opposing lists.
/// Adding a TModule to a TOwner's Input will add it to the other TOwner's Output list, and vice versa.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TModule"></typeparam>

[Serializable]
public abstract class ARX_IOList<TModule>
{
    [SerializeField]
    public List<TModule> Inputs = new List<TModule>();
    public List<TModule> GetInputs
    {
        get
        {
            if (Inputs == null) Inputs = new List<TModule>();
            return Inputs;
        }
    }

    [SerializeField]
    public List<TModule> Outputs = new List<TModule>();
    public List<TModule> GetOutputs
    {
        get
        {
            if (Outputs == null) Outputs = new List<TModule>();
            return Outputs;
        }
    }

    [SerializeField]
    [HideInInspector]
    private TModule owner;
    public TModule GetOwner { get { return owner; } }

    public ARX_IOList(TModule oOwner) { owner = oOwner; }

    /// <summary>
    /// Returns a null TModule value 
    /// </summary>
    /// <returns></returns>
    public abstract TModule GetNullModule();
    protected abstract List<TModule> GetInputList(TModule other);
    protected abstract List<TModule> GetOutputList(TModule other);
    protected abstract bool IsEqual(TModule one, TModule two);

    public TModule[] GetInputArray { get {
            if (Inputs == null)
                Inputs = new List<TModule>();
            return Inputs.ToArray(); } }
    public TModule[] GetOutputArray { get {
            if (Outputs == null)
                Outputs = new List<TModule>();
            return Outputs.ToArray(); } }

    public TModule GetInput(int index) {
        if (index < 0 || index >= Inputs.Count)
            return GetNullModule();
        return Inputs[index];
    }

    public TModule GetOutput(int index)
    {
        if (index < 0 || index >= Outputs.Count)
            return GetNullModule();
        return Outputs[index];
    }

    public void AddInput(TModule other) {
        //Add other to this list's inputs
        //Add this owner to other module's outputs
        List<TModule> thisInputs = Inputs;
        List<TModule> otherOutputs = GetOutputList(other);

        AddSecure(other, thisInputs);
        AddSecure(owner, otherOutputs);

    }
    public void AddOutput(TModule other) {

        //Add other to this list's outputs
        //Add this owner to other module's inputs

        List<TModule> thisOutputs = Outputs;
        List<TModule> otherInputs = GetInputList(other);

        AddSecure(other, thisOutputs);
        AddSecure(owner, otherInputs);
    }
    public void RemoveInput(TModule other) {
        //Remove other from this inputs
        //Remove owner from other outputs
        List<TModule> thisInputs = Inputs;
        List<TModule> otherOutputs = GetOutputList(other);

        thisInputs.Remove(other);
        otherOutputs.Remove(owner);
    }
    public void RemoveOutput(TModule other) {
        //Remove other from this outputs
        //Remove owner from other inputs
        List<TModule> thisOutputs = Outputs;
        List<TModule> otherInputs = GetInputList(other);

        thisOutputs.Remove(other);
        otherInputs.Remove(owner);
    }

    /// <summary>
    /// Checks to see if list already contains item before adding it.
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="list"></param>
    void AddSecure(TModule mod, List<TModule> list)
    {
        if (IsEqual(mod, GetNullModule()))
            return;

        if (list.Contains(mod))
            return;
        list.Add(mod);
    }

    /// <summary>
    /// Removes null values from inputs and outputs
    /// </summary>
    public void Validate()
    {
        while (GetInputs.Contains(GetNullModule()))
        {
            Inputs.Remove(GetNullModule());
        }

        while (GetOutputs.Contains(GetNullModule()))
        {
            Outputs.Remove(GetNullModule());
        }
    }

    public void Clear()
    {
        while (GetInputs.Count > 0)
            RemoveInput(Inputs[0]);


        while (GetOutputs.Count > 0)
            RemoveInput(Outputs[0]);
    }

    public void SetOwner(TModule oNewOwner)
    {
        owner = oNewOwner;
    }

}
