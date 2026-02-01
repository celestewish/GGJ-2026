using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPositionManager : MonoBehaviour
{
    [SerializeField] EndPosition[] endPositions;

    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if(gameManager != null )
        {
            PlayerData[] playerPositions = gameManager.GetGameEndPlayerPositions();
            for( int i = 0; i < playerPositions.Length; i++ )
            {
                if (playerPositions[i].playerIndex != -1)
                {
                    endPositions[i].SetCharacter(playerPositions[i].characterData);
                } else
                {
                    endPositions[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ReturnToMainMenu()
    {
        gameManager.LoadMainMenu();
    }
}
