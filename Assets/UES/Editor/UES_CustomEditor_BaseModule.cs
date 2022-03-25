using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARX;

[CustomEditor(typeof(UES_BaseModule), true)]
public class UES_CustomEditor_BaseModule : Editor
{
    #region Variables

    Color PowerInputColor = Color.red;
    Color PowerOutputColor = Color.blue;
    Color TriggerInputColor = Color.yellow;
    Color TriggerOutputColor = Color.green;

    int nTriggerWidth = 75, nTriggerHeight = 75, nBodyWidth = 275, nBodyHeight = 300;

    Rect GetRect
    {
        get
        {
            int height = 600;
            return GUILayoutUtility.GetRect(300, height);
        }
    }

    //UES_BaseModule GetTarget { get { return (UES_BaseModule)target; } }
    #endregion

    #region Serialized Properties

    


    SerializedObject baseModuleProperty;

    SerializedProperty powerLightProperty, triggerOutputProperty, triggerInputProperty,
        powerInputProperty, powerOutputProperty;

    SerializedProperty uesActiveProperty;
    SerializedProperty poweredProperty;
    SerializedProperty uesModuleProperty;
    SerializedProperty modelsProperty;

    UES_Light mo_powerLight { set { powerLightProperty.objectReferenceValue = value; } }
    GameObject mo_triggerOutputModel { set { triggerOutputProperty.objectReferenceValue = value; } }
    GameObject mo_triggerInputModel { set { triggerInputProperty.objectReferenceValue = value; } }
    GameObject mo_powerOutputModel { set { powerOutputProperty.objectReferenceValue = value; } }
    GameObject mo_powerInputModel { set { powerInputProperty.objectReferenceValue = value; } }
    UES_BaseModule_AttachedModules uesModules
    {
        get
        {
            return (UES_BaseModule_AttachedModules)uesModuleProperty.objectReferenceValue;
        }
    }

    UES_BaseModule_CompModels models
    {
        get
        {
            return (UES_BaseModule_CompModels)modelsProperty.objectReferenceValue;
        }
    }

    UES_BaseModule baseModule
    {
        get
        {
            return (UES_BaseModule)baseModuleProperty.targetObject;
        }
    }

    #endregion

    private void OnEnable()
    {
        baseModuleProperty = new SerializedObject(target);

        uesActiveProperty = baseModuleProperty.FindProperty("mb_isUESModuleActive");
        poweredProperty = baseModuleProperty.FindProperty("mb_isPowered");
        uesModuleProperty = baseModuleProperty.FindProperty("uesModules");


        modelsProperty = baseModuleProperty.FindProperty("models");
        powerLightProperty = modelsProperty.FindPropertyRelative("mo_powerLight");
        triggerOutputProperty = modelsProperty.FindPropertyRelative("mo_triggerOutputModel");
        triggerInputProperty = modelsProperty.FindPropertyRelative("mo_triggerInputModel");
        powerInputProperty = modelsProperty.FindPropertyRelative("mo_powerOutputModel");
        powerOutputProperty = modelsProperty.FindPropertyRelative("mo_powerInputModel");

    }

    public override void OnInspectorGUI()
    {
        RectGuide guide = new RectGuide(GetRect);

        EditorGUI.DrawRect(guide.BoundingRect, Color.black);

        DrawTitle(guide);
        DrawTopTriggers(guide);
        DrawBody(guide);
        DrawTrashCan(guide);
        DrawBottomTriggerInput(guide);

        base.OnInspectorGUI();

    }

    #region Draw

    void DrawTitle(RectGuide guide)
    {
        GUI.Label(guide.GetNextRect(150), target.GetType().ToString());
        guide.NewLine(2);

    }

    void DrawTopTriggers(RectGuide guide)
    {
        int nInbetween = 75, nPadding = 75;
        guide.MoveLastRect(nPadding);

        Rect rectTriggerOut = guide.GetNextRect(nTriggerWidth, nTriggerHeight);
        guide.MoveLastRect(nInbetween);


        Rect rectPowerOut = guide.GetNextRect(nTriggerWidth, nTriggerHeight);

        EditorGUI.DrawRect(rectTriggerOut, Color.blue);
        EditorGUI.DrawRect(rectPowerOut, Color.green);

        guide.NewLineByHeight(nTriggerHeight);

        RunPowerOut(rectPowerOut);
        RunTriggerOut(rectTriggerOut);
    }

    void DrawBody(RectGuide guide)
    {

        int nPadding = 50;
        guide.MoveLastRect(nPadding);
        Rect rectBody = guide.GetNextRect(nBodyWidth, nBodyHeight);
        EditorGUI.DrawRect(rectBody, Color.gray);


        //Draw Innards
        RectGuide innerBody = new RectGuide(rectBody);

        //On Off
        int nOnOffWidth = 100, nOnOffHeight = 50;
        innerBody.MoveLastRect(10, 10);
        Rect rectOnOff = innerBody.GetNextRect(nOnOffWidth, nOnOffHeight);
        EditorGUI.DrawRect(rectOnOff, Color.white);
        innerBody.NewLine(2);
        RunOnOffSwitch(rectOnOff);

        //Power Light
        int nPowerLightWidth = 60, nPowerLightHeight = 150;
        innerBody.MoveLastRect(10, 50);
        Rect rectPowerLight = innerBody.GetNextRect(nPowerLightWidth, nPowerLightHeight);
        EditorGUI.DrawRect(rectPowerLight, Color.white);
        RunPowerLight(rectPowerLight);

        //Components Models
        innerBody.MoveLastRect(10);
        int nComponentWidth = 190, nComponentHeight = 200;
        Rect rectComponent = innerBody.GetNextRect(nComponentWidth, nComponentHeight);
        EditorGUI.DrawRect(rectComponent, Color.black);
        RunComponentModels(rectComponent);

        //Power Input
        int nPowerInputWidth = 50, nPowerInputHeight = 250, nYOffset = 75;
        innerBody.Return();
        innerBody.MoveLastRect(-nPowerInputWidth, -nYOffset);
        Rect rectPowerInput = innerBody.GetNextRect(nPowerInputWidth, nPowerInputHeight);
        EditorGUI.DrawRect(rectPowerInput, Color.red);
        RunPowerInput(rectPowerInput);


    }

    void DrawBottomTriggerInput(RectGuide guide)
    {

        int nYOffset = 25, nXOffset = 75;
        guide.NewLineByHeight(nBodyWidth + nYOffset);
        guide.MoveLastRect(nXOffset);
        Rect rectTriggerIn = guide.GetNextRect(nTriggerWidth, nTriggerHeight);
        EditorGUI.DrawRect(rectTriggerIn, Color.cyan);
        RunTriggerIn(rectTriggerIn);
    }

    void DrawTrashCan(RectGuide guide)
    {
        RectGuide innerGuide = new RectGuide(guide.BoundingRect);

        int nYOffset = 150, nXOffset = 175, nWidth = 150, nHeight = 150;
        innerGuide.NewLineByHeight(nBodyWidth + nYOffset);
        innerGuide.MoveLastRect(nXOffset);
        Rect rectTrashCan = innerGuide.GetNextRect(nWidth, nHeight);

        RunTrashCan(rectTrashCan);
    }

    #endregion

    #region Run

    void RunTrashCan(Rect rect)
    {
        EditorGUI.DrawRect(rect, Color.gray);

        //THe ObjectField function stops working when given a label. How strange.
        //GUIContent label = new GUIContent("Trash", "Removes the given module from " +  name + "'s Input and Output Lists.");

        UES_BaseModule mod = (UES_BaseModule)EditorGUI.ObjectField(rect, null, typeof(UES_BaseModule), true);

        if (mod != null)
        {
            uesModules.RemoveModule(mod);
        }


        rect.y -= 16;
        GUI.Label(rect, "Remove Mini Component");
    }
    void RunOnOffSwitch(Rect rect)
    {
        Rect On = new Rect(rect.x, rect.y, rect.width / 2, rect.height);
        Rect Off = new Rect(rect.x + On.width, rect.y, On.width, On.height);

        if (GUI.Button(On, "On"))
        {
            uesActiveProperty.boolValue = true;
        }
        else if (GUI.Button(Off, "Off"))
        {
            uesActiveProperty.boolValue = false;
        }

        if (uesActiveProperty.boolValue == true)
        {
            EditorGUI.DrawRect(On, Color.green);
            EditorGUI.DrawRect(Off, Color.black);
        }
        else
        {
            EditorGUI.DrawRect(On, Color.black);
            EditorGUI.DrawRect(Off, Color.red);
        }


        rect.y += 32;
        GUI.Label(rect, "Power Switch");

    }
    void RunPowerLight(Rect rect)
    {
        Color c;
        if (poweredProperty.boolValue == true)
            c = Color.green;
        else
            c = Color.red;

        EditorGUI.DrawRect(rect, c);


        rect.y -= 32;
        GUI.Label(rect, "Power\n" + (poweredProperty.boolValue ? "ON" : "Off"));
    }
    void SetColor(Object obj)
    {
        if (obj == null)
            GUI.color = Color.red;
        else
            GUI.color = Color.green;
    }

    void RunComponentModels(Rect rect)
    {
        int nRows = 5;
        int nLabelHeight = 16;
        int nHeight = ((int)rect.height / nRows) - nLabelHeight;
        RectGuide guide = new RectGuide(rect, nHeight);

        GUI.Label(guide.GetNextRect(100), "Power Out Model");
        guide.NewLineByHeight(nLabelHeight);
        SetColor(models.mo_powerOutputModel);
        mo_powerOutputModel =
            (GameObject)EditorGUI.ObjectField(guide.GetNextRect(rect.width, nHeight), models.mo_powerOutputModel, typeof(GameObject), true);
        guide.NewLine();
        GUI.color = Color.white;

        GUI.Label(guide.GetNextRect(100), "Trigger Out Model");
        guide.NewLineByHeight(nLabelHeight);
        SetColor(models.mo_triggerOutputModel);
        mo_triggerOutputModel =
            (GameObject)EditorGUI.ObjectField(guide.GetNextRect(rect.width, nHeight), models.mo_triggerOutputModel, typeof(GameObject), true);
        guide.NewLine();
        GUI.color = Color.white;

        GUI.Label(guide.GetNextRect(100), "Power Input Model");
        guide.NewLineByHeight(nLabelHeight);
        SetColor(models.mo_powerInputModel);
        mo_powerInputModel =
            (GameObject)EditorGUI.ObjectField(guide.GetNextRect(rect.width, nHeight), models.mo_powerInputModel, typeof(GameObject), true);
        guide.NewLine();
        GUI.color = Color.white;

        GUI.Label(guide.GetNextRect(100), "Trigger Input Model");
        guide.NewLineByHeight(nLabelHeight);
        SetColor(models.mo_triggerInputModel);
        mo_triggerInputModel =
            (GameObject)EditorGUI.ObjectField(guide.GetNextRect(rect.width, nHeight), models.mo_triggerInputModel, typeof(GameObject), true);
        guide.NewLine();
        GUI.color = Color.white;

        GUI.Label(guide.GetNextRect(100), "Power Light Model");
        guide.NewLineByHeight(nLabelHeight);
        SetColor(models.mo_powerLight);
        mo_powerLight =
            (UES_Light)EditorGUI.ObjectField(guide.GetNextRect(rect.width, nHeight), models.mo_powerLight, typeof(UES_Light), true);
        guide.NewLine();
        GUI.color = Color.white;
    }

    void RunPowerInput(Rect rect)
    {
        GUI.color = PowerInputColor;
        UES_BaseModule mod = (UES_BaseModule)EditorGUI.ObjectField(rect, null, typeof(UES_BaseModule), true);

        if (mod != null)
        {
            
            uesModules.AddPowerInput(mod);
        }
        GUI.color = Color.white;


        rect.y -= 16;
        GUI.Label(rect, "Power\nIn");
    }

    void RunTriggerOut(Rect rect)
    {

        GUI.color = TriggerOutputColor;
        UES_BaseModule mod = (UES_BaseModule)EditorGUI.ObjectField(rect, null, typeof(UES_BaseModule), true);

        if (mod != null)
        {
            uesModules.AddTriggerOutput(mod);
        }
        GUI.color = Color.white;


        rect.y -= 16;
        GUI.Label(rect, "Trigger Out");
    }
    void RunTriggerIn(Rect rect)
    {
        GUI.color = Color.yellow;
        UES_BaseModule mod = (UES_BaseModule)EditorGUI.ObjectField(rect, null, typeof(UES_BaseModule), true);

        if (mod != null)
        {
            uesModules.AddTriggerInput(mod);
        }
        GUI.color = Color.white;


        rect.y -= 16;
        GUI.Label(rect, "Trigger In");
    }
    void RunPowerOut(Rect rect)
    {
        GUI.color = PowerOutputColor;
        UES_BaseModule mod = (UES_BaseModule)EditorGUI.ObjectField(rect, null, typeof(UES_BaseModule), true);

        if (mod != null)
        {
            uesModules.AddPowerOutput(mod);
        }
        GUI.color = Color.white;

        rect.y -= 16;
        GUI.Label(rect, "Power Out");
    }

    #endregion

}
