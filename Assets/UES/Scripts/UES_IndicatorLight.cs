using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class UES_IndicatorLight : MonoBehaviour
{
    public bool isPowered = false, isPoweredThisFrame = false;
    public Material onMaterial, offMaterial;

    Material myMaterial;
    Material GetMaterial
    {
        get
        {
            if (myMaterial == null)
            {
                Renderer ren = GetComponent<Renderer>();
                if (ren != null)
                    myMaterial = ren.material;
            }
            return myMaterial;
        }

        set
        {
            Renderer ren = GetComponent<Renderer>();
            if (ren != null)
                ren.material = value;
        }
    }

    public void OnFixedUpdate()
    {
        if (isPowered || isPoweredThisFrame)
            On();
        else
            Off();
    }

    public  void OnPowered()
    {
        On();
    }


    public void Depower()
    {
        Off();
    }

    public void On()
    {
        isPoweredThisFrame = true;
        isPowered = true;
        GetMaterial = onMaterial;
    }

    public void Off()
    {
        isPoweredThisFrame = false;
        isPowered = false;
        GetMaterial = offMaterial;
    }
}
