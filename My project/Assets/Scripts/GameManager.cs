using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabArray;
    private CharacterParentClass[] activePlayerArray = new CharacterParentClass[4];
    private PlayerData[] pDataArray = new PlayerData[2];
    public string gameSceneName;

    private GameState currentState;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentState = GameState.Menu;
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

    int playerIndex;
    string controlScheme;
    InputDevice[] devices;

    int charID;
}

public enum GameState
{
    Menu, Game, Results, Pause
}
