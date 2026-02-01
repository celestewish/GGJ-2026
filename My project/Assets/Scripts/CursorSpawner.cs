using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSpawner : MonoBehaviour
{

    [SerializeField] PlayerInputManager manager;
    [SerializeField] GameObject cursorPrefab;

    GameManager gameManager;

    public void SpawnCursors(PlayerData[] pDatas)
    {
        if (manager == null)
        {
            manager = FindFirstObjectByType<PlayerInputManager>();
        }

        manager.EnableJoining();
        manager.playerPrefab = cursorPrefab;

        for (int i = 0; i < pDatas.Length; i++)
        {
            if (pDatas[i].playerIndex != -1)
            {
                manager.JoinPlayer(pDatas[i].playerIndex, -1, pDatas[i].controlScheme, pDatas[i].devices);
            }
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manager == null)
        {
            manager = FindFirstObjectByType<PlayerInputManager>();
        }

        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            //manager.onPlayerJoined += gameManager.playerJoin;
            print("Subscribed playerJoin to onPlayerJoined");

            PlayerData[] pDatas = gameManager.GetPlayerData();

            bool initialized = false;
            for (int i = 0; i < pDatas.Length; i++)
            {
                if (pDatas[i].playerIndex != -1)
                {
                    initialized = true;
                }
            }

            if (initialized)
            {
                //manager.onPlayerJoined += gameManager.playerJoin;

                SpawnCursors(pDatas);
            }
        }
    }

    public void CursorSpawned(PlayerInput input)
    {
        SelectionCursor s = input.gameObject.GetComponent<SelectionCursor>();
        s.transform.SetParent(FindFirstObjectByType<Canvas>().transform);

        (s.transform as RectTransform).anchoredPosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    /*private void OnDisable()
    {
        if (gameManager)
        {
            manager.onPlayerJoined -= gameManager.playerJoin;
        }
    }*/
}
