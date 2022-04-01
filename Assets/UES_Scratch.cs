using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UES_Scratch : MonoBehaviour
{

    public XNode.SceneGraph sceneGraph;
    public bool RunTest = false;
    private void Update()
    {

        if (RunTest)
        {
            RunTest = false;
            sceneGraph.graph.Clear();
            UES_NodeTreeParser parser = new UES_NodeTreeParser(transform, sceneGraph);
            parser.Run();
        }
    }
}
