using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


public class UES_Contraption : SceneGraph
{
    public override void ReactToOpen()
    {
        graph.Clear();
        UES_NodeTreeParser parser = new UES_NodeTreeParser(transform, this);
        parser.Run();
    }
}


