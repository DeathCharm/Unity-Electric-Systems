using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Keyboard : UES_BaseModule
{
    public KeyCode targetKey = KeyCode.Space;

    public enum ClickType { ButtonDown, ButtonUp, ButtonHold }
    public ClickType clickType = ClickType.ButtonDown;
    public enum ClickReaction { Trigger, Power }
    public ClickReaction actionOnClick = ClickReaction.Trigger;

    public TMPro.TMP_Text text;

    public override void OnFixedUpdate()
    {
        if (text != null)
            text.text = targetKey.ToString();
        base.OnFixedUpdate();
    }

    public override void OnPowered()
    {
        base.OnPowered();
        if (actionOnClick == ClickReaction.Trigger && ClickSatisfied())
            SendTrigger(signal);
    }

    public override void SendPowerToModules(UES_Signal signal)
    {
        if(actionOnClick == ClickReaction.Power && ClickSatisfied())
            base.SendPowerToModules(signal);
    }

    bool ClickSatisfied()
    {
        
            switch (clickType)
            {
                case ClickType.ButtonDown:
                    if (Input.GetKeyDown(targetKey))
                        return true;
                    break;
                case ClickType.ButtonHold:
                    if (Input.GetKeyDown(targetKey))
                        return true; 
                    if (Input.GetKey(targetKey))
                        return true;
                    break;
                case ClickType.ButtonUp:
                    if (Input.GetKeyUp(targetKey))
                        return true;
                    break;
            }
        
        return false;
    }

}
