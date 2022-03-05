using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Display : MonoBehaviour
{
    public string strText = "Text";
    public Color colro = Color.black;
    public TMPro.TMP_Text textMesh;
    public void ChangeDisplay(string strText, Color color)
    {
        textMesh.text = strText;
        textMesh.color = color;
    }
}
