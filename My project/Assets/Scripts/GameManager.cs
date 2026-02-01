using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabArray;
    public Vector2[] spawnPositions; //Define in inspector
    private CharacterParentClass[] activePlayerArray = new CharacterParentClass[4];
    private PlayerData[] pDataArray = new PlayerData[4];
    public string gameSceneName;

    private GameState currentState = GameState.Menu;

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
        if(currentState == GameState.Game)
        {
            populatePlayersInGame();
        }
    }

    public void playerJoin(PlayerInput p)
    {
        pDataArray[p.playerIndex] = new PlayerData(p.playerIndex, p.currentControlScheme, p.devices.ToArray());

        SelectionCursor s = p.gameObject.GetComponent<SelectionCursor>();
        s.setControlScheme(p.currentControlScheme);
        s.transform.SetParent(FindFirstObjectByType<Canvas>().transform);
        s.transform.position = Vector3.zero;
    }

    public void startTheGame()
    {
        currentState = GameState.Game;
        //add position spawning code here
        SceneManager.LoadScene(gameSceneName);

    }

    public void populatePlayersInGame()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefabArray[pDataArray[i].charID]);
            activePlayerArray[pDataArray[i].playerIndex] = newPlayer.GetComponent<CharacterParentClass>();
            newPlayer.transform.position = spawnPositions[i];
        }
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
    
    public void pointerEnteredMask()
    {
        print("Entered mask");
    }

}

public struct PlayerData
{
    public PlayerData(int p, string c, InputDevice[] d)
    {
        playerIndex = p;
        controlScheme = c;
        devices = d;
        charID = -1;
    }

    public int playerIndex;
    public string controlScheme;
    public InputDevice[] devices;

    public int charID;
}


public enum GameState
{
    Menu, Game, Results, Pause
}
