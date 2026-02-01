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
    string cScheme;
    public int sensitivity = 10;
    private Mouse vMouse;
    private RectTransform rt;
    private RectTransform canvasRT;
    private GraphicRaycaster raycaster;

    Dictionary<int, GameObject> hoveringObjects = new Dictionary<int, GameObject>();

    public void OnNavigate(InputValue iv)
    {
        nav = iv.Get<Vector2>();
        Vector2 currentPos = nav;
        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            currentPos = vMouse.position.ReadValue();
            Vector2 newPos = currentPos + nav * (sensitivity * 10) * Time.deltaTime;
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
            }
        }
    }


    private void OnEnable()
    {
        pi = GetComponent<PlayerInput>();
        rt = GetComponent<RectTransform>();
        Canvas canvas = FindFirstObjectByType<Canvas>();
        canvasRT = canvas.GetComponent<RectTransform>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();

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

    public void setControlScheme(string s)
    {
        cScheme = s;
    }

    private void updateCursorPosition(Vector2 currentPosition)
    {
        switch (cScheme)
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
