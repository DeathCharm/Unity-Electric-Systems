using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

public class UES_NodeTreeParser : QuickUnityTreeParser
    {
    public UES_NodeTreeParser(Transform oRoot, SceneGraph oGraph) : base(oRoot)
    {
        mo_graph = oGraph;
    }

    static SceneGraph mo_graph;
    static float nfNodeWith = 275, nfNodeHeight = 200;

    Dictionary<UES_BaseModule, UES_ModuleNode> nodeDictionary = new Dictionary<UES_BaseModule, UES_ModuleNode>();

    enum ConnectionType { Trigger, Power };

    public override bool IsValidGridMovement(Transform to)
    {
        if (to == null)
            return false;

        if (to.GetComponent<UES_BaseModule>() != null)
            return true;
        return false;
    }

    public override void ActOnObject(Transform target)
    {
        //Only create nodes for with a UES_BaseModule component
       UES_BaseModule module = target.GetComponent<UES_BaseModule>();
        if (module == null)
            return;

        //Create a node for the module and save it to the dictionary
        UES_ModuleNode node = mo_graph.graph.AddNode<UES_ModuleNode>();
        node.position = new Vector2(gridX * nfNodeWith, gridY * nfNodeHeight);
        node.name = "UES Module (" + gridX + ", " + gridY + ")";
        target.name = node.name;

        

        node.SetOwningModule(module);

        nodeDictionary[module] = node;

        Debug.Log("Creating node for " + node.name);
    }

    public override void OnParseEnd(Transform target)
    {
        Debug.Log("Ending parse on " + target.name);

        //For each module in the node dictionary
        foreach (UES_BaseModule mod in nodeDictionary.Keys)
        {
            //For each connection output(trigger and power), get the connected node
            CreateConnections(mod, mod.GetPowerOutputs, ConnectionType.Power);
            CreateConnections(mod, mod.GetTriggerOutputs, ConnectionType.Trigger);
        }
    }

    void CreateConnections(UES_BaseModule mod, UES_BaseModule[] oaMods, ConnectionType eType)
    {
        UES_ModuleNode thisNode = nodeDictionary[mod];

        foreach (UES_BaseModule m in oaMods)
        {
            UES_ModuleNode other = nodeDictionary[m];

            if (eType == ConnectionType.Trigger)
            {
                //Get the NodePorts for setConnection and addConnection
                NodePort setPort = thisNode.GetPort("setTriggerOutput");
                NodePort addPort = other.GetPort("addTriggerInput");
                setPort.Connect(addPort);
                addPort.Connect(setPort);
            }
            else if (eType == ConnectionType.Power)
            {
                NodePort setPort = thisNode.GetPort("setPowerOutput");
                NodePort addPort = other.GetPort("addPowerInput");
                setPort.Connect(addPort);
                addPort.Connect(setPort);
            }
        }
    }

    public override void OnChildFound(Transform parent, Transform child)
    {

    }

    public override void OnSiblingFound(Transform elder, Transform younger)
    {

    }

    public override void OnParentSiblingFound(Transform uncle, Transform nephew)
    {

    }

    public override void OnNavigateBackOneLevel(Transform from, Transform to)
    {

    }

    
}
