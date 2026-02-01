using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerSpawn : MonoBehaviour
{

    int index = 0;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] PlayerUIManager specialBarManager;
    PlayerInputManager manager;

    PlayerData[] playerDatas;

    CharacterParentClass[] players;

     void Start()
    {
        //manager = GetComponent<PlayerInputManager>();
        /*index = 0;
        manager.playerPrefab = players[index];*/
    }

    public CharacterParentClass[] SpawnPlayers(PlayerData[] playerDatas, GameObject[] characterPrefabs)
    {
        if(manager == null)
        {
            manager = GetComponent<PlayerInputManager>();
            manager.EnableJoining();
        } 

        this.playerDatas = playerDatas;

        players = new CharacterParentClass[playerDatas.Length];

        for (int i = 0; i < playerDatas.Length; i++)
        {
            PlayerData current = playerDatas[i];
            if (current.playerIndex != -1)
            {
                print($"Spawn player {current.playerIndex}");
                manager.playerPrefab = characterPrefabs[current.charID];
                manager.JoinPlayer(current.playerIndex, -1, current.controlScheme, current.devices);
            }
        }

        manager.DisableJoining();

        return players;
    }

    public void SwitchtoNextSpawnCharacter(PlayerInput input)
    {
        CharacterParentClass spawnedCharacter = input.gameObject.GetComponent<CharacterParentClass>();
        spawnedCharacter.SetPlayerData(playerDatas[input.playerIndex]);

        spawnedCharacter.transform.position = spawnPositions[input.playerIndex].position;
        spawnPositions[input.playerIndex].gameObject.SetActive(false);

        players[input.playerIndex] = spawnedCharacter;

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
