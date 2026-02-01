using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabArray;
    public Vector2[] spawnPositions; //Define in inspector
    public PlayerInputManager manager;
    private CharacterParentClass[] activePlayerArray = new CharacterParentClass[4];
    private PlayerData[] pDataArray = new PlayerData[4];
    private int playersReady = 0;
    private int playersInGame = 0;
    public string gameSceneName = "GameScene";

    private int playersStillAlive = 0;
    private PlayerData[] remainingPlayers;
    private PlayerData[] gameEndPositions = new PlayerData[4];

    List<SelectionCursor> cursors = new List<SelectionCursor>();

    private GameState currentState = GameState.Menu;

    public bool IsGameReady()
    {
        return playersReady == playersInGame && playersInGame >= 2;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        print("Started scene");
        if (currentState == GameState.Game)
        {
            populatePlayersInGame();
            remainingPlayers = pDataArray;
            playersStillAlive = playersInGame;
        }
        else if (currentState == GameState.Results)
        {

        }
    }

    private void Start()
    {
        for (int i = 0; i < pDataArray.Length; i++)
        {
            if (pDataArray[i].devices == null)
            {
                pDataArray[i] = new PlayerData(-1, "", null);
                gameEndPositions[i] = new PlayerData(-1, "", null);
            }
        }

        /*if(currentState == GameState.Game)
        {
            populatePlayersInGame();
        }*/
    }

    public void playerJoin(PlayerInput p)
    {
        pDataArray[p.playerIndex] = new PlayerData(p.playerIndex, p.currentControlScheme, p.devices.ToArray());

        SelectionCursor s = p.gameObject.GetComponent<SelectionCursor>();
        s.transform.SetParent(FindFirstObjectByType<Canvas>().transform);

        cursors.Add(s);

        playersInGame++;
        //s.transform.position = Vector3.zero;

        s.SetGameManager(this);
    }

    public void startTheGame()
    {
        currentState = GameState.Game;

        for (int i = 0; i < cursors.Count; i++)
        {
            cursors[i].gameObject.SetActive(false);
        }

        //add position spawning code here
        SceneManager.LoadScene(gameSceneName);
    }

    public void populatePlayersInGame()
    {
        PlayerSpawn playerSpawn = FindFirstObjectByType<PlayerSpawn>();
        if(playerSpawn)
        {
            activePlayerArray = playerSpawn.SpawnPlayers(pDataArray, playerPrefabArray);
        }

        /*for(int i = 0; i < 4; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefabArray[pDataArray[i].charID]);
            activePlayerArray[pDataArray[i].playerIndex] = newPlayer.GetComponent<CharacterParentClass>();
            newPlayer.transform.position = spawnPositions[i];
        }*/
    }

    public void setPlayer(int mask, int playerWhoChose)
    {
        GameObject p = Instantiate(playerPrefabArray[mask]);
        activePlayerArray[playerWhoChose] = p.GetComponent<CharacterParentClass>();
    }

    public void placeObjectAt(Transform p ,Vector2 pos) //truthfully i dont know if this function will need to be as general as it is, but it cant hurt, use this to move objects to certain positions.
    {
        p.position = pos;
    }

    public void changeGameState(GameState gs) //just in case this needs to be called externally
    {
        currentState = gs;
    }

    public void modifyPlayerStat(int player, int stat, int value, bool addative)
    {
        activePlayerArray[player].changeStat(stat, value, addative);
    }

    public void KnockoutPlayer(PlayerData player)
    {
        print("Knockout Player");
        gameEndPositions[playersStillAlive - 1] = player;
        remainingPlayers[player.playerIndex] = new PlayerData(-1, "", null);
        playersStillAlive--;

        if(playersStillAlive == 1)
        {
            for(int i = 0; i < remainingPlayers.Length; i++)
            {
                if (remainingPlayers[i].playerIndex != -1)
                {
                    print(remainingPlayers[i].playerIndex + " - " + remainingPlayers[i].characterData.characterName);
                    gameEndPositions[playersStillAlive - 1] = remainingPlayers[i];
                    print(gameEndPositions[playersStillAlive - 1].playerIndex + " - " + gameEndPositions[playersStillAlive - 1].characterData.characterName);
                    break;
                }
            }

            //handle game end
            currentState = GameState.Results;
            SceneManager.LoadScene("EndScene");
        }
    }
    public PlayerData[] GetGameEndPlayerPositions()
    {
        return gameEndPositions;
    }

    public bool SetPlayerSelection(int playerIndex, CharacterData data)
    {
        if (pDataArray[playerIndex].charID == -1)
        {
            pDataArray[playerIndex].charID = data.characterID;
            pDataArray[playerIndex].characterData = data;
            playersReady++;

            return true;
        }
        return false;
    }
    public void UnsetPlayerSelection(int playerIndex)
    {
        pDataArray[playerIndex].charID = -1;
        pDataArray[playerIndex].characterData = null;
        playersReady--;
    }


    public void PauseGame()
    {
        currentState = GameState.Pause;
        Time.timeScale = 0f;
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetPaused(true);
    }

    public void ResumeGame()
    {
        currentState = GameState.Game;
        Time.timeScale = 1f;
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetPaused(false);
    }
}

[System.Serializable]
public struct PlayerData
{
    public PlayerData(int p, string c, InputDevice[] d)
    {
        playerIndex = p;
        controlScheme = c;
        devices = d;
        charID = -1;
        characterData = null;
    }

    public int playerIndex;
    public string controlScheme;
    public InputDevice[] devices;

    public int charID;
    public CharacterData characterData;
}


public enum GameState
{
    Menu, Game, Results, Pause
}
