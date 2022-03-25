using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
 public class DestroyMe : MonoBehaviour
    {
    /// <summary>
    /// Safety switch to prevent accidental deletion of objects.
    /// Set true to call DestroyImmediate on this gameobject
    /// </summary>
    public bool enableDestruction = false;
    private void Update()
    {
        if(enableDestruction)
            DestroyImmediate(gameObject);
    }
}

