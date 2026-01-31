using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerSpawn : MonoBehaviour
{

    int index = 0;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    PlayerInputManager manager;

     void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = 0;
        manager.playerPrefab = players[index];
    }
    public void SwitchtoNextSpawnCharacter(PlayerInput input)
    {
        index++;
        manager.playerPrefab = players[index];
    }


    /*public Transform[] spawnPoints;
    private int PlayerSpawnCount;

    public void OnPlayerJoined(PlayerInput Control)
    {
        Control.transform.position = spawnPoints[PlayerSpawnCount].transform.position;
        if(PlayerSpawnCount == 0)
        {
          //  Control.GetComponent<PlayerController>().SwitchOnCamera();
        }
        PlayerSpawnCount++;
    }*/
}
