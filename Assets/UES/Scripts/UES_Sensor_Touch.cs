using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers when colliding with a Player or a gameobject tagged with a tergeted tag.
/// After triggering, a light goes green for a short time.
/// </summary>
public class UES_Sensor_Touch : UES_BaseModule
{
    public bool targetPlayer = true;
    public List<string> targetTags = new List<string>();
    public UES_Indicator indicator;

    private void OnCollisionEnter(Collision collision)
    {
        if (!mb_isPowered)
            return;

        if (targetTags.Contains(collision.gameObject.tag))
        {
            SendTrigger(signal);
            indicator.ActivateIndicator();
            return;
        }

        if (collision.gameObject.tag == "Player" && targetPlayer == true)
        {
            SendTrigger(signal);
            indicator.ActivateIndicator();
            return;
        }
    }
}
