using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class SelectionCursor : MonoBehaviour
{
    Vector2 nav;
    PlayerInput pi;
    public int sensitivity = 10;
    private Mouse vMouse;
    private RectTransform rt;
    private RectTransform canvasRT;
    private GraphicRaycaster raycaster;

    Dictionary<int, GameObject> hoveringObjects = new Dictionary<int, GameObject>();

    private GameManager gameManager;
    private MainMenuManager mainMenuManager;

    private CharacterData characterData;

    private MaskSelection maskSelection;

    public void OnNavigate(InputValue iv)
    {
        nav = iv.Get<Vector2>();
        Vector2 currentPos = nav;
        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            currentPos = vMouse.position.ReadValue();
            Vector2 newPos = currentPos + nav * (sensitivity * 5) * Time.deltaTime;

            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

            InputState.Change(vMouse.position, newPos);
            InputState.Change(vMouse.delta, nav);
   
            currentPos = newPos;
            //print(currentPos);
        }

        updateCursorPosition(currentPos);

        var pointer = new PointerEventData(EventSystem.current) { position = transform.position };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointer, raycastResults);
        Dictionary<int, GameObject> newHoveringIDs = new Dictionary<int, GameObject>();
        foreach (RaycastResult raycastResult in raycastResults)
        {
            var ui = raycastResult.gameObject.GetComponent<UIBehaviour>();
            if (ui)
            {
                int id = raycastResult.gameObject.GetHashCode();
                newHoveringIDs.Add(id, raycastResult.gameObject);
                if(!hoveringObjects.ContainsKey(id))
                {
                    hoveringObjects.Add(id, raycastResult.gameObject);
                }
                Debug.Log($"Entered on {raycastResult.gameObject}");
                ExecuteEvents.Execute(raycastResult.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
            }
        }

        int[] oldIDs = hoveringObjects.Keys.ToArray();
        for (int i = 0; i < oldIDs.Length; i++)
        {
            if (!newHoveringIDs.ContainsKey(oldIDs[i]))
            {
                GameObject obj = hoveringObjects[oldIDs[i]];
                var ui = obj.GetComponent<UIBehaviour>();
                if (ui)
                {
                    Debug.Log($"Exited on {obj}");
                    ExecuteEvents.Execute(obj, pointer, ExecuteEvents.pointerExitHandler);
                }
            }
            hoveringObjects.Remove(oldIDs[i]);
        }
    }

    public void OnClick(InputValue iv)
    {
        var pointer = new PointerEventData(EventSystem.current) { position = transform.position };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        raycaster.Raycast(pointer, raycastResults);
        foreach (RaycastResult raycastResult in raycastResults)
        {
            var ui = raycastResult.gameObject.GetComponent<UIBehaviour>();
            if (ui)
            {
                Debug.Log($"Clicked on {raycastResult.gameObject}");
                ExecuteEvents.Execute(raycastResult.gameObject, pointer, ExecuteEvents.pointerClickHandler);

                if (this.maskSelection == null)
                {
                    print("No Mask Selection");
                    MaskSelection maskSelection = raycastResult.gameObject.transform.parent.GetComponent<MaskSelection>();
                    if (maskSelection)
                    {
                        print("Found Mask Selection");
                        CharacterData characterData = maskSelection.GetCharacterData();

                        if (gameManager.SetPlayerSelection(pi.playerIndex, characterData))
                        {
                            print("Mask Set Successfully");
                            this.maskSelection = maskSelection;
                            maskSelection.MaskSelected(pi.playerIndex);

                            mainMenuManager.CheckGameReady();
                        }
                    }
                }
            }
        }
    }

    public void OnCancel(InputValue iv)
    {
        if(this.maskSelection != null)
        {
            this.maskSelection.MaskUnselected();
            this.maskSelection = null;
            this.characterData = null;

            gameManager.UnsetPlayerSelection(pi.playerIndex);

            mainMenuManager.CheckGameReady();
        }
    }


    private void OnEnable()
    {
        pi = GetComponent<PlayerInput>();
        rt = GetComponent<RectTransform>();
        Canvas canvas = FindFirstObjectByType<Canvas>();
        canvasRT = canvas.GetComponent<RectTransform>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();

        mainMenuManager = FindFirstObjectByType<MainMenuManager>();

        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            if (vMouse == null)
            {
                vMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (vMouse.added == false)
            {
                InputSystem.AddDevice(vMouse);
            }

            InputUser.PerformPairingWithDevice(vMouse, pi.user);
            InputState.Change(vMouse.position, rt.anchoredPosition);
            print(vMouse.position.ReadValue() + "from enable");
        }
  
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(vMouse);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void updateCursorPosition(Vector2 currentPosition)
    {
        switch (pi.currentControlScheme)
        {
            case "Keyboard&Mouse":
                transform.position = nav;
                break;
            case "Gamepad":
                Vector2 anchPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, currentPosition, null, out anchPos);
                rt.anchoredPosition = anchPos;
                break;
        }
    }

}
