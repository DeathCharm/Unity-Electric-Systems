using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_WireFramer : UES_BaseModule
{

    public enum WireFrameAction {Invert, SetToOff }
    public WireFrameAction wireFrameAction = WireFrameAction.Invert;
    public override void OnTriggered(UES_Signal signal)
    {
        base.OnTriggered(signal);
        foreach (UES_BaseModule mod in GetTriggerOutputs)
        {
            SetToWireFrame(mod.gameObject);
        }
    }

    void SetToWireFrame(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        Renderer ren = obj.GetComponent<Renderer>();

        switch (wireFrameAction)
        {
            case WireFrameAction.Invert:
                if (col != null)
                    col.enabled = !col.enabled;
                if(ren != null)
                    ren.enabled = col.enabled;
                break;
            case WireFrameAction.SetToOff:
                if (col != null)
                    col.enabled = false;
                if (ren != null)
                    ren.enabled = false;
                break;
        }
    }
}
