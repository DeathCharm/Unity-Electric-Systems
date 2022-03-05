using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UES_ClickMe : UES_BaseModule
{
    Color bufRendererColor;
    public Renderer mo_buttonRenderer;

    Renderer _renderer;
    Renderer GetRenderer
    {
        get
        {
            if (mo_buttonRenderer != null)
                return mo_buttonRenderer;
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
            return _renderer;
        }
    }

    public void ClickMe()
    {
        Debug.Log("Clicked.");
        SendTrigger(signal);
    }

    public void MouseOnMe() {
        bufRendererColor = GetRenderer.material.color;
        GetRenderer.material.color = Color.green;
        Debug.Log(name + " is now green.");
    }

    public void MouseOffMe() {
        GetRenderer.material.color = bufRendererColor;
    }
}
