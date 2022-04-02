using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;


//Wiring
public partial class UES_BaseModule : MonoBehaviour
{

    #region Wires

    [HideInInspector]
    public bool mb_createWires = false;

    [SerializeField]
    GameObject mo_wireRoot;

    [HideInInspector]
    public UES_WireRope[] moa_wires = new UES_WireRope[0];

    [SerializeField]
    public Dictionary<UES_BaseModule, UES_WireRope> wireDictionary = new Dictionary<UES_BaseModule, UES_WireRope>();

    /// <summary>
    /// Returns the game object used as the root of this object's connected wire models
    /// </summary>
    public GameObject GetWireRoot
    {
        get
        {
            //If a direct child already exists with a UES_WireRoot script
            if (mo_wireRoot == null)
            {
                UES_WireRoot childWireRoot = GetComponentInChildren<UES_WireRoot>();
                if (childWireRoot != null && childWireRoot.transform.parent == transform)
                {
                    mo_wireRoot = childWireRoot.gameObject;
                }
            }

            //If no wireRoot exists in children
            if (mo_wireRoot == null)
            {
                mo_wireRoot = new GameObject();
                mo_wireRoot.name = "Wire Root";
                mo_wireRoot.transform.SetParent(transform);
                mo_wireRoot.transform.localPosition = Vector3.zero;
                mo_wireRoot.AddComponent<UES_WireRoot>();
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
        UES_WireAnchor[] childRopes = GetComponentsInChildren<UES_WireAnchor>(true);
        for (int i = 0; i < childRopes.Length; i++)
        {
            bool bPointsToNoAnchor = true;
            foreach (UES_BaseModule mod in GetPowerOutputs)
            {
                if (childRopes[i].anchorTarget == mod)
                {
                    bPointsToNoAnchor = false;
                    break;
                }
            }
            foreach (UES_BaseModule mod in GetTriggerOutputs)
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

        UES_WireAnchor[] anchors = GetComponentsInChildren<UES_WireAnchor>(true);
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
        //Debug.Log(name + " is Creating Wires.");

        foreach (UES_BaseModule mod in GetPowerOutputs)
        {
            Debug.Log(mod.name + " checking " + mod.name + "'s wires.");
            if (HasWireInChildren(mod))
            {
                Debug.Log(mod.name + " already has a wire from " + name + " pointing at it.");
                continue;
            }
            else
                CreateNewWire(mod);
        }
        foreach (UES_BaseModule mod in GetTriggerOutputs)
        {
            Debug.Log(mod.name + " checking " + mod.name + "'s wires.");
            if (HasWireInChildren(mod))
            {
                Debug.Log(mod.name + " already has a wire from " + name + " pointing at it.");
                continue;
            }
            else
                CreateNewWire(mod);
        }
    }
    #endregion

}

