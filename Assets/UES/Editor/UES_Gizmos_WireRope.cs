using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(UES_WireRope))]
public class UES_Gizmos_WireRope : Editor
{

    public UES_WireRope GetTarget { get { return (UES_WireRope)target; } }
    private void OnSceneGUI()
    {
        for (int i = 0; i < GetTarget.ropeLocalPositions.Count; i++)
        {
            Vector3 vecLocal = GetTarget.ropeLocalPositions[i];

            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            Vector3 vecWorldPosition = vecLocal + GetTarget.transform.position;

            Vector3 movement = Handles.PositionHandle(vecWorldPosition, rotation);
            movement -= GetTarget.transform.position;

            GetTarget.ropeLocalPositions[i] = movement;

        }
    }
}
