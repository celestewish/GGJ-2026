using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabArray;
    private CharacterParentClass[] activePlayerArray = new CharacterParentClass[4]; //the positions in this array correspond to the players. 0 is player 1 and so on
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

public enum GameState
{
    Menu, Game, Results, Pause
}
