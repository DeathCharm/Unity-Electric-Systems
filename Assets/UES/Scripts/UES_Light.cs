using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class UES_Light : UES_BaseModule
{
    public Material onMaterial, offMaterial;

    Material myMaterial;
    Material GetMaterial
    {
        get
        {
            if (myMaterial == null)
            {
                Renderer ren = GetComponent<Renderer>();
                if(ren != null)
                    myMaterial = ren.material;
            }
            return myMaterial;
        }

        set
        {
            Renderer ren = GetComponent<Renderer>();
            if(ren != null)
                ren.material = value;
        }
    }

    public override void OnFixedUpdate()
    {
        if (isPowered || isPoweredThisFrame)
            On();
        else
            Off();
    }

    public override void OnPowered()
    {
        base.OnPowered();
        On();
    }

    public override void OnPowerLost()
    {
        base.OnPowerLost();
        
    }

    public override void Depower()
    {
        base.Depower();
        Off();
        Debug.Log("DePowered");
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
