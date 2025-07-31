using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputSystem_Actions InputActions;

    void Awake()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();
    }

    public static void PausePlayerInputs()
    {
        InputActions.Player.Disable();
    }

    public static Vector2 MovementInputValue()
    {
        return InputActions.Player.Move.ReadValue<Vector2>();
    }

    void OnDestroy() => InputActions.Disable();
}