using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_Speaker : UES_BaseModule
{
    public AudioSource audioSource;
    public bool interruptSelf = false;

    public override void OnTriggered(UES_Signal signal)
    {
        base.OnTriggered(signal);


        if (audioSource != null)
        {
            //If no sound is playing
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
                return;
            }

            //If sound is playing but it can interrupt itself to replay
            if (audioSource.isPlaying && interruptSelf == true)
            {
                audioSource.Play();
                return;
            }
        }
    }
}
