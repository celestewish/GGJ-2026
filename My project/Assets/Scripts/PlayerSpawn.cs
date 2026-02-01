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

        //Stash the device that was used to spawn this character
        InputDevice device = input.GetDevice<InputDevice>();
        CharacterParentClass spawnedCharacter = input.gameObject.GetComponent<CharacterParentClass>();
        spawnedCharacter.controllerID = device.displayName == "Keyboard" || device.displayName == "Mouse" ? "Mouse" : "Controller";

        manager.playerPrefab = players[Mathf.Min(players.Count - 1, index)];
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
