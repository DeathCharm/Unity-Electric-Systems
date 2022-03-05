using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChildPositioner : MonoBehaviour
{
    public Vector3 offset, padding;

    public int columns = 1;

    public int Validate(int n)
    {
        if (n < 0)
            n = 0;

        return n;
    }

    private void Update()
    {
        columns = Validate(columns);


        int i = 0;
        foreach (Transform child in transform)
        {
            if (child == transform)
                continue;

            int nColumn = 0;
            if(i > 0)
                nColumn = i % columns;

            int nRow = 0;
            if (i > 0)
                nRow = i / columns;


            Vector3 pos = new Vector3();
            pos.x = offset.x * i;
            pos.y = offset.y * nRow;
            pos.z = offset.z * nColumn;

            pos += padding;
            child.transform.localPosition = pos;
            i++;
        }
    }
}
