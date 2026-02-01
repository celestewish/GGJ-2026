using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] playerPrefabArray;
    public Vector2[] spawnPositions; //Define in inspector
    public PlayerInputManager manager;
    private CharacterParentClass[] activePlayerArray = new CharacterParentClass[4];
    private PlayerData[] pDataArray = new PlayerData[4];
    private int playersReady = 0;
    private int playersInGame = 0;
    public string gameSceneName;

    private int playersStillAlive = 0;
    private PlayerData[] remainingPlayers;
    private PlayerData[] gameEndPositions = new PlayerData[4];

    List<SelectionCursor> cursors = new List<SelectionCursor>();

    private GameState currentState = GameState.Menu;

    private bool gameStarting = false;

    public bool IsGameReady()
    {
        return playersReady == playersInGame && playersInGame >= 2;
    }

    public PlayerData[] GetPlayerData()
    {
        return pDataArray;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        manager = GetComponent<PlayerInputManager>();

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
            gameStarting = false;

            manager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            manager.enabled = false;

            StartCoroutine(populatePlayersInGame());

            remainingPlayers = new PlayerData[4];
            for (int i = 0; i < pDataArray.Length; i++)
            {
                PlayerData copy = new PlayerData();
                copy.playerIndex = pDataArray[i].playerIndex;
                copy.controlScheme = pDataArray[i].controlScheme;
                copy.devices = pDataArray[i].devices;
                copy.charID = pDataArray[i].charID;
                copy.characterData = pDataArray[i].characterData;
                remainingPlayers[i] = copy;
            }
            playersStillAlive = playersInGame;
        }
        else if (currentState == GameState.Results)
        {
            gameStarting = false;
            manager.joinBehavior = PlayerJoinBehavior.JoinPlayersManually;
            manager.enabled = true;
        }
        else if (currentState == GameState.Menu)
        {
            gameStarting = false;

            manager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            manager.enabled = true;

            cursors = new List<SelectionCursor>();
            playersReady = 0;

            for (int i = 0; i < pDataArray.Length; i++)
            {
                pDataArray[i].charID = -1;
                pDataArray[i].characterData = null;
            }
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

    public void OnPlayerJoined(PlayerInput p)
    {
        print("Player Joined Message");
    }
    public void playerJoin(PlayerInput p)
    {
        if(currentState != GameState.Menu && currentState != GameState.Results)
        {
            return;
        }

        if (pDataArray[p.playerIndex].playerIndex == -1)
        {
            print("New Player Joined");
            pDataArray[p.playerIndex] = new PlayerData(p.playerIndex, p.currentControlScheme, p.devices.ToArray());
            playersInGame++;
        }

        SelectionCursor s = p.gameObject.GetComponent<SelectionCursor>();
        if (s != null)
        {
            s.transform.SetParent(FindFirstObjectByType<Canvas>().transform);

            (s.transform as RectTransform).anchoredPosition = new Vector2(Screen.width / 2, Screen.height / 2);

            cursors.Add(s);


            //s.transform.position = Vector3.zero;

            s.SetGameManager(this);
        }
    }

    public void startTheGame()
    {
        if (!gameStarting)
        {
            gameStarting = true;

            currentState = GameState.Game;

            for (int i = 0; i < cursors.Count; i++)
            {
                cursors[i].gameObject.SetActive(false);
            }

            //add position spawning code here
            manager.enabled = false;
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public IEnumerator populatePlayersInGame()
    {
        yield return new WaitForSeconds(0.1f);

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
        print(player.playerIndex + ", " + player.controlScheme + ", " + player.charID);
        gameEndPositions[playersStillAlive - 1] = player;
        print("Before");
        for (int i = 0; i < gameEndPositions.Length; i++)
        {
            if (gameEndPositions[i].playerIndex != -1)
                print(gameEndPositions[i].playerIndex + " - " + gameEndPositions[i].characterData.characterName);
        }
        remainingPlayers[player.playerIndex] = new PlayerData(-1, "", null);
        print("After");
        for (int i = 0; i < gameEndPositions.Length; i++)
        {
            if (gameEndPositions[i].playerIndex != -1)
                print(gameEndPositions[i].playerIndex + " - " + gameEndPositions[i].characterData.characterName);
        }
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

            for (int i = 0; i < gameEndPositions.Length; i++)
            {
                if (gameEndPositions[i].playerIndex != -1)
                    print(gameEndPositions[i].playerIndex + " - " + gameEndPositions[i].characterData.characterName);
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
        print(pDataArray[playerIndex].charID + " - " + playerIndex);
        if (pDataArray[playerIndex].charID == -1)
        {
            //ensure only one character type per game
            for (int i = 0;i < pDataArray.Length;i++)
            {
                if (pDataArray[i].charID == data.characterID)
                {
                    return false;
                }
            }

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

    public void LoadMainMenu()
    {
        currentState = GameState.Menu;
        SceneManager.LoadScene("MainMenu");
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
