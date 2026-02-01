using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class SelectionCursor : MonoBehaviour
{
    Vector2 nav;
    PlayerInput pi;
    string cScheme;
    public int sensitivity = 10;
    private Mouse vMouse;
    private RectTransform rt;
    private RectTransform canvasRT;
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
            print(currentPos);
        }

        updateCursorPosition(currentPos);
    }

    private void OnEnable()
    {
        pi = GetComponent<PlayerInput>();
        rt = GetComponent<RectTransform>();
        canvasRT = FindFirstObjectByType<Canvas>().GetComponent<RectTransform>();

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
