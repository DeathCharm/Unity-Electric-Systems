using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UES_Delay : UES_BaseModule
{
    public float delayTime;
    public Image progressPanel;
    float timeElasped;

    private void Start()
    {
        timeElasped = 0;
        SetPanelFill(0);

    }

    public override void OnTriggered(UES_Signal signal)
    {
        base.OnTriggered(signal);
        timeElasped = 0;
        isUESModuleActive = true;
    }

    public override void OnPowered()
    {
        base.OnPowered();
        Tick();
    }

    public void Tick()
    {
        timeElasped += Time.deltaTime;

        float fractionFinished = timeElasped / delayTime;

        float r, g, b;
        r = Mathf.Lerp(Color.red.r, Color.green.r, fractionFinished );
        g = Mathf.Lerp(Color.red.g, Color.green.g, fractionFinished );
        b = Mathf.Lerp(Color.red.b, Color.green.b, fractionFinished);

        SetPanelColor(r, g, b);
        SetPanelFill(fractionFinished);

        if (fractionFinished >= 1)
        {
            timeElasped = 0;
            isUESModuleActive = false;
            SendTrigger(signal);
            SetPanelColor(Color.yellow);
        }
    }

    void SetPanelColor(float r, float g, float b)
    {
        if (progressPanel == null)
            return;

        progressPanel.color = new Color(r, g, b, 1);
    }

    void SetPanelColor(Color color)
    {
        SetPanelColor(color.r, color.g, color.b);
    }

    void SetPanelFill(float fill)
    {
        if (progressPanel == null)
            return;

        progressPanel.fillAmount = fill;
    }
}
