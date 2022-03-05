using UnityEngine;
using UnityStandardAssets.Utility;
using System;


[Serializable]
public class WaypointList
{
    public WaypointList() { }
    public WaypointCircuit circuit;
    public Transform[] items = new Transform[0];
}