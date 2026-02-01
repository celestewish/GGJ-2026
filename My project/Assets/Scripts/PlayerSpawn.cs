using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerSpawn : MonoBehaviour
{

    int index = 0;
    [SerializeField] List<GameObject> players = new List<GameObject>();
    [SerializeField] Transform[] spawnPositions;
    PlayerInputManager manager;

    PlayerData[] playerDatas;

     void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        /*index = 0;
        manager.playerPrefab = players[index];*/
    }

    public void SpawnPlayers(PlayerData[] playerDatas, GameObject[] characterPrefabs)
    {
        if(manager == null)
        {
            manager = GetComponent<PlayerInputManager>();
            manager.EnableJoining();
        } 

        this.playerDatas = playerDatas;
        for (int i = 0; i < playerDatas.Length; i++)
        {
            PlayerData current = playerDatas[i];
            if (current.playerIndex != -1)
            {
                manager.playerPrefab = characterPrefabs[current.charID];
                manager.JoinPlayer(current.playerIndex, -1, current.controlScheme, current.devices);
            }
        }

        manager.DisableJoining();
    }

    public void SwitchtoNextSpawnCharacter(PlayerInput input)
    {
        CharacterParentClass spawnedCharacter = input.gameObject.GetComponent<CharacterParentClass>();
        spawnedCharacter.SetPlayerData(playerDatas[input.playerIndex]);

        spawnedCharacter.transform.position = spawnPositions[input.playerIndex].position;

        /*index++;

        //Stash the device that was used to spawn this character
        InputDevice device = input.GetDevice<InputDevice>();
        InputDevice[] devices = input.devices.ToArray();
        CharacterParentClass spawnedCharacter = input.gameObject.GetComponent<CharacterParentClass>();
        spawnedCharacter.controllerID = device.displayName == "Keyboard" || device.displayName == "Mouse" ? "Mouse" : "Controller";

        manager.playerPrefab = players[Mathf.Min(players.Count - 1, index)];*/
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
