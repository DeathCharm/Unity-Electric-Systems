using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UES_Signal
{
    public List<string> tags;

    public UES_Signal(List<string> astrTags) {
        tags = astrTags;
    }


    public bool Contains(string tag)
    {
        return tags.Contains(tag);
    }

}
