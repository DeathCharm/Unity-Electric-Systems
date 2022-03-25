using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


public class UES_BlueprintGraph : SceneGraph
{
    public override void ReactToOpen()
    {
        Debug.Log(name + " was just opened.");

    }

}
