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

    public string GetGridPosition { get { return "(" + gridX + ", " + gridY + ")"; } }

    public int gridX = 0;
    public int gridY = 0;

    public abstract T GetChild(T target);
    public abstract T GetSibling(T target);
    public abstract T GetParent(T target);
    public abstract void ActOnObject(T target);
    public abstract bool IsVoid(T target);

    public abstract bool IsEqual(T one, T two);
    public abstract bool IsValidGridMovement(T to);
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
            if (gridX < 0)
            {

                Debug.Log("Parse at index less than zero. " + GetGridPosition);
                OnParseEnd(obj);
                return;
            }
            //If the target is void, return
            if (IsVoid(obj))
            {
                Debug.Log("Parse ended due to void object at " + GetGridPosition);
                return;
            }
            //Act on the target
            ActOnObject(obj);

            //Get target's child
            T child = GetChild(obj);

            //If target has a child, set as target and loop
            if (IsVoid(child) == false)
            {
                if (IsValidGridMovement(child))
                {
                    gridX++;
                }
                Debug.Log("Found child at " + GetGridPosition);
                OnChildFound(obj, child);
                obj = child;
                continue;
            }
            //If target has no child, do nothing 
            else
            {
                Debug.Log("Found no child of " + GetGridPosition);
                //Intentionally blank
                OnNoChildFound(obj);
            }

            //Get target's sibling
            T sibling = GetSibling(obj);
            //If sibling found, set as target and loop
            if (IsVoid(sibling) == false)
            {

                if (IsValidGridMovement(sibling))
                {
                    gridY++;
                }
                Debug.Log("Found sibling at " + GetGridPosition);
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
                    Debug.Log("Navigated back to " + GetGridPosition + ". Ending Parse.");
                    OnParseEnd(obj);
                    return;
                }

                //Else, get parent of target
                Debug.Log("No sibling found of " + GetGridPosition);
                OnNoSiblingFound(obj);

                //Loop backwards until a parent with a sibling is found or a void parent is found
                while (true)
                {
                    T parent = GetParent(obj);
                    //If no parent, return
                    if (IsVoid(parent))
                    {
                        Debug.Log("No parent found for " + GetGridPosition + ". Ending Parse.");
                        OnNoParentFound(obj);
                        OnParseEnd(obj);
                        return;
                    }
                    //Else if parent found, get parent's sibling
                    else
                    {
                        T parentSibling = GetSibling(parent);
                        //If the parent's sibling was found
                        if (IsVoid(parentSibling) == false)
                        {
                            if (IsValidGridMovement(parentSibling))
                            {
                                gridX--;
                                gridY++;
                            }
                            Debug.Log("Found parent sibling at " + GetGridPosition);
                            OnParentSiblingFound(parentSibling, obj);
                            obj = parentSibling;
                            break;
                        }
                        //Else If no parent sibling found, navigate backwards to parent
                        else
                        {
                            if (IsValidGridMovement(parent))
                            {
                                gridX--;
                            }
                            Debug.Log("No parent sibling found for " + GetGridPosition + ". Navigating back to parent.");
                            OnNavigateBackOneLevel(obj, parent);
                            obj = parent;
                            continue;

                        }
                    }
                }


            }
        }

        Debug.Log("Ended parse naturally. This should never occur.");
    }

}
