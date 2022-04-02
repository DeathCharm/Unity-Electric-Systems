using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public partial class UES_BaseModule : MonoBehaviour
{

    #region Draw

    public virtual void DrawUESGizmo()
    {

    }

    public void OnDrawGizmos()
    {
        //Draw Trigger outputs
        foreach (UES_BaseModule mod in GetTriggerOutputs)
        {
            if (mod == null)
                continue;

            if (mod.mo_triggerInputModel != null && mo_triggerOutputModel != null)
            {
                DrawGizmoLine(mo_triggerOutputModel, mod.mo_triggerInputModel, GetActiveColor);
            }
            else
            {
                Debug.LogError("Missing trigger input or output model for " + mod.name + " or " + name);
            }
        }

        foreach (UES_BaseModule mod in GetPowerOutputs)
        {
            if (mod == null)
            {
                continue;
            }

            if (mod.mo_powerInputModel != null && mo_powerOutputModel != null)
            {
                DrawGizmoLine(mo_powerOutputModel, mod.mo_powerInputModel, GetActiveColor);
            }
            else
            {
                //Debug.LogError("Missing power input or output model for " + mod.name + " or " + name);
            }

        }
    }


    void DrawGizmoLine(GameObject o1, GameObject o2, Color color)
    {
        Color cBuf = color;
        Gizmos.color = color;
        Gizmos.DrawLine(o2.transform.position, o1.transform.position);
        Gizmos.color = cBuf;
    }
    #endregion

}

