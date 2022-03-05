using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


    public static class UES
    {



    public static Color playerColor = Color.white;
    public static GameObject player;
    public static Rigidbody playerRigidbody;

    public static GameObject GetPlayer
    {
        get
        {
            if(player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            return player;
        }
    }


    public static void SetPlayerPosition(Vector3 position)
    {
                GetPlayer.transform.position = position;
        
    }

    public static void WinLevel()
    {
        Debug.Log("Yey! You won the level!");
    }
    public static void ResetLevel()
    {
        UES_Signal resetSignel = new UES_Signal();

        Debug.Log("Resetting level");
        UES_BaseModule[] allMods = GameObject.FindObjectsOfType<UES_BaseModule>();
        foreach (UES_BaseModule mod in allMods)
            mod.OnReset(resetSignel);
    }
    public static float PlayerSpeed
    {
        get
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                return 0;
            }

            if (playerRigidbody == null)
                playerRigidbody = player.GetComponent<Rigidbody>();

            if (playerRigidbody == null)
            {
                return 0;
            }

            return playerRigidbody.velocity.magnitude;
        }
    }
        public static void KillPlayer(string str)
        {
            Debug.Log("Oh no, the player is dead! Killed by: " + str);
        }

    public static void ColorPlayer(Color color)
    {
        Debug.Log("Changing player color to " + color);
        playerColor = color;
    }
    }

