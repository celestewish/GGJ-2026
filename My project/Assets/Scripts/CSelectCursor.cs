using UnityEngine;

public class CSelectCursor : MonoBehaviour
{
    public RectTransform[] slots;
    public MainMenuManager m;
    public int columns = 4;         

    int index = 0;
    int playerId = 0;
    public RectTransform cursor;
    private bool hasSelected;

    void Awake()
    {
        cursor = GetComponent<RectTransform>();
        MoveCursor();
    }

    private void Start()
    {
        m = GameObject.Find("UI").GetComponent<MainMenuManager>();
    }

    void Update()
    {
        int row = index / columns;
        int col = index % columns;

        //this should be updated for the controller
        if (!hasSelected)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) col++;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) col--;
            if (Input.GetKeyDown(KeyCode.DownArrow)) row++;
            if (Input.GetKeyDown(KeyCode.UpArrow)) row--;

            col = Mathf.Clamp(col, 0, columns - 1);
            row = Mathf.Clamp(row, 0, (slots.Length - 1) / columns);

            int newIndex = row * columns + col;
            if (newIndex < slots.Length)
            {
                index = newIndex;
                MoveCursor();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Return)) //change for controller
        {
            toggleCharacterSelection();
        }
    }

    void MoveCursor()
    {
        cursor.position = slots[index].position;
    }

    void toggleCharacterSelection()
    {
        if (!hasSelected)
        {
            Debug.Log("Selected: " + slots[index].name);
            m.updateCharacterSelected(playerId, slots[index].name);
            hasSelected = true;
        }
        else
        {
            Debug.Log("Deselected");
            m.updateCharacterSelected(playerId, "None");
            hasSelected = false;
        }
        
    }
}
