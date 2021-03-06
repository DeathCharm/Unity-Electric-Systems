using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A parser for unity components that uses the transform component to navigate through
/// child transforms. 
/// </summary>
public class QuickUnityTreeParser : ARX_TreeParser<UnityEngine.Transform>
{
    public QuickUnityTreeParser(Transform start) : base(start){ }

    public override bool IsValidGridMovement(Transform to)
    {

        return true;
    }

    public override bool IsEqual(Transform one, Transform two)
    {
        return one == two;
    }

    public override Transform GetChild(Transform target)
    {

        if (target.transform.childCount > 0)
        {
            return target.GetChild(0);
        }
        else
            return null;
    }

    public override Transform GetParent(Transform target)
    {
        return target.parent;
    }

    public override Transform GetSibling(Transform target)
    {
        int i = target.GetSiblingIndex();
        i++;

        if (target.parent == null || i >= target.parent.childCount)
            return null;
        else
            return target.parent.GetChild(i);
    }

    public override bool IsVoid(Transform target)
    {
        if (target == null)
            return true;
        return false;
    }

    public override void ActOnObject(Transform target)
    {

    }
}
