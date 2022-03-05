using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UES_PlayerSpawn : UES_BaseModule
{

    public override void OnTriggered(UES_Signal signal)
    {
        MovePlayer();
    }

    public override void OnPowered()
    {
        MovePlayer();
    }

    void MovePlayer() {

        UES.SetPlayerPosition(transform.position);
    }

}
