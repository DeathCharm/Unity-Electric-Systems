using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Navigates a nested data structure and acts on its leaf nodes.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ARX_TreeParser<T>
{

    public ARX_TreeParser(T start)
    {
        obj = start;
        firstGiven = start;
    }

    public T obj;
    protected T firstGiven;

    private int gridX = 0;
    private int gridY = 0;

    public abstract T GetChild(T target);
    public abstract T GetSibling(T target);
    public abstract T GetParent(T target);
    public abstract void ActOnObject(T target);
    public abstract bool IsVoid(T target);

    public abstract bool IsEqual(T one, T two);
    public virtual void OnParseStart(T target) { }
    public virtual void OnParseEnd(T target) { }

    public virtual void OnNoChildFound(T parent) { }
    public virtual void OnNoSiblingFound(T onlyChild) { }
    public virtual void OnNoParentFound(T orphan) { }
    public virtual void OnChildFound(T parent, T child) { }
    public virtual void OnSiblingFound(T elder, T younger) { }
    public virtual void OnParentSiblingFound(T uncle, T nephew) { }

    public virtual void OnNavigateBackOneLevel(T from, T to) { }

    public void Run()
    {
        gridX = 0;
        gridY = 0;

        OnParseStart(obj);
        //Loop to here when acting on a new target
        while (true)
        {
            //If the target is void, return
            if (IsVoid(obj))
                return;

            //Act on the target
            ActOnObject(obj);

            //Get target's child
            T child = GetChild(obj);

            //If target has a child, set as target and loop
            if (IsVoid(child) == false)
            {
                gridX++;
                OnChildFound(obj, child);
                obj = child;
                continue;
            }
            //If target has no child, do nothing 
            else
            {
                //Intentionally blank
                OnNoChildFound(obj);
            }

            //Get target's sibling
            T sibling = GetSibling(obj);
            //If sibling found, set as target and loop
            if (IsVoid(sibling) == false)
            {
                gridY++;
                OnSiblingFound(obj, sibling);
                obj = sibling;
                continue;
            }
            //Else if no sibling,
            //check if target is the start object.
            else
            {
                //If target is the first given object, return
                if (IsEqual(obj, firstGiven))
                {
                    OnParseEnd(obj);
                    return;
                }

                //Else, get parent of target

                OnNoSiblingFound(obj);

                //Loop backwards until a parent with a sibling is found or a void parent is found
                while (true)
                {
                    T parent = GetParent(obj);
                    //If no parent, return
                    if (IsVoid(parent))
                    {
                        OnNoParentFound(obj);
                        OnParseEnd(obj);
                        return;
                    }
                    //Else if parent found, get parent's sibling
                    else
                    {
                        T parentSibling = GetSibling(parent);
                        if (IsVoid(parentSibling))
                        {
                            //If no parent sibling found, navigate backwards to parent
                            gridX--;
                            OnNavigateBackOneLevel(obj, parent);
                            obj = parent;
                            continue;
                        }
                        //Else if parent sibling found, set as target and loop
                        else
                        {
                            gridX--;
                            gridY++;
                            OnParentSiblingFound(parentSibling, obj);
                            obj = parentSibling;
                            break;
                        }
                    }
                }

            }
        }


    }

}
