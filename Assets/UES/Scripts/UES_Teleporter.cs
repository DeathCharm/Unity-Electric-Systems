using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class UES_Teleporter : UES_BaseModule
{
    public GameObject endPoint, ringRoot;
    public AutoMoveAndRotate[] rotators;

    MeshRenderer[] renderers;
    MeshRenderer[] GetRenderers {

        get{
            if (renderers == null)
                renderers = ringRoot.GetComponentsInChildren<MeshRenderer>();
            return renderers;

        } }

    public override void OnTriggered(UES_Signal signal)
    {
        base.OnTriggered(signal);
        if (endPoint != null)
        {
            UES.SetPlayerPosition(endPoint.transform.position);

        }
    }

    public override void OnPowered()
    {
        base.OnPowered();
        foreach (AutoMoveAndRotate rot in rotators)
        {
            rot.enabled = true;
        }
        foreach (MeshRenderer ren in GetRenderers)
        {
            if (ren.gameObject == this.gameObject)
                continue;
            if (Application.isPlaying)
                ren.material.SetColor("_Color", Color.green);
        }
    }

    public override void OnInactive()
    {
        base.OnInactive();
        foreach (AutoMoveAndRotate rot in rotators)
        {
            rot.enabled = false;
        }
        foreach (MeshRenderer ren in GetRenderers)
        {
            if (ren.gameObject == this.gameObject)
                continue;
            if(Application.isPlaying)
            ren.material.SetColor("_Color", Color.red);
        }
    }

    public override void DrawUESGizmo()
    {
        base.DrawUESGizmo();
        if (endPoint != null)
        {
            Gizmos.DrawLine(transform.position, endPoint.transform.position);
            Gizmos.DrawSphere(endPoint.transform.position, 0.3F);
        }
    }


}
