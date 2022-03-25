using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

public class UES_NodeTreeParser : QuickUnityTreeParser
    {
    public UES_NodeTreeParser(Transform oRoot, SceneGraph oGraph) : base(NodeAction, oRoot)
    {
        mo_graph = oGraph;
    }

    static SceneGraph mo_graph;
    static float x = 0, y = 0, nfNodeWith = 275, nfNodeHeight = 150;

    public static void NodeAction(Transform trans) 
    {
        Node node = mo_graph.graph.AddNode<UES_ModuleNode>();
        node.position = new Vector2(x, y);
    }

    public override void OnChildFound(Transform parent, Transform child)
    {
        x += nfNodeWith;
    }

    public override void OnSiblingFound(Transform elder, Transform younger)
    {
        y += nfNodeHeight;
    }

    public override void OnParentSiblingFound(Transform uncle, Transform nephew)
    {
        x -= nfNodeWith;
        y += nfNodeHeight;
    }

    public override void OnNavigateBackOneLevel(Transform from, Transform to)
    {
        x -= nfNodeWith;
    }

    public override void OnParseEnd(Transform target)
    {
        x = 0; y = 0;
    }
}
