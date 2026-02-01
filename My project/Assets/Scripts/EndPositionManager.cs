using UnityEngine;

public class EndPositionManager : MonoBehaviour
{
    [SerializeField] EndPosition[] endPositions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

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
}
