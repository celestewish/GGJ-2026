using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] panelList;
    private GameObject activePanel;
    private string[] selectedCharacters = new string[4]; //the plain text names of the selected characters
    private bool[] charSelected =  new bool[4];
    public bool allSelected;
    public GameObject allSelectedPanel;

    private GameManager g;
    private MenuState m;

    private void Start()
    {
        g = FindFirstObjectByType<GameManager>();

        if (g == null)
        {
            Debug.LogWarning("No GameManager Found!");
        }

        m = MenuState.Main;
        activePanel = panelList[(int)m];
    }

    public void togglePanel(int p)
    {
        activePanel.SetActive(false);
        activePanel = panelList[p];
        activePanel.SetActive(true);
        m = (MenuState)p;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void updateCharacterSelected(int i, string name)
    {
        charSelected[i] = !charSelected[i];
        if (charSelected[i])
        {
            selectedCharacters[i] = name;
        }
        else
        {
            selectedCharacters[i] = "None";
        }

        if (checkIfAllReady())
        {
            allSelectedPanel.SetActive(true);
        }
        else
        {
            allSelectedPanel.SetActive(false);
        }
    }

    bool checkIfAllReady()
    {
        foreach(bool b in charSelected)
        {
            if (b == false) return false;
        }
        return true;
    }

    public void CheckGameReady()
    {
        if(g.IsGameReady())
        {
            allSelectedPanel.SetActive(true);
        }
        else
        {
            allSelectedPanel.SetActive(false);
        }
    }

    public void startTheGame()
    {
        g.startTheGame();
    }
}

public enum MenuState //is this needed? no. Do i want it? yeah
{
    Main, Settings, Character
}
