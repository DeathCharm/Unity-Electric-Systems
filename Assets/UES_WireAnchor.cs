using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UES_WireAnchor : MonoBehaviour
{

    public GameObject anchorTarget;
    private void Update()
    {
        if (anchorTarget == null)
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
}
