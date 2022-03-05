using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Scratch : MonoBehaviour
{
    public GameObject oOtherThing;

    public void OnDrawGizmos()
    {
        if (oOtherThing == null)
            return;
        Gizmos.DrawLine(transform.position, oOtherThing.transform.position);
    }
}
