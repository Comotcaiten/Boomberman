using UnityEngine;

public class MoveInputController : InputController
{
    public enum MoveControlType
    {
        Joystick,
        Dpad,
        Keyboard
    }

    public MoveControlType moveType;

    public Joystick joystick;
    public Dpad dpad;

    protected override void Start()
    {
        if (GameManager.Instance != null)
        {
            moveType = GameManager.Instance.inputSettings.moveControlType; // Get the move control type from GameManager
            Debug.Log("MoveInputController: Move control type set to " + moveType);
            UpdateUIState();
        }
        else
        {
            Debug.Log("IputController-Move: GameManager is not initialized. Please bind the player before using input controls.");
        }
    }

    void Update()
    {

        if (player != null && GameManager.Instance != null)
        {
            Vector2 input = Vector2.zero;
            if (PlatformUtils.IsMobilePlatform())
            {
                if (useControllerMoblieOnly || moveType == MoveControlType.Keyboard)
                {
                    Debug.Log("Using mobile controller only, but move type is not set to Joystick. Defaulting to Joystick input.");
                    GameManager.Instance.inputSettings.moveControlType = MoveControlType.Joystick; // Update the setting
                    moveType = MoveControlType.Joystick; // Update the local variable
                    UpdateUIState(); // Update the UI state
                }
                input = MoblieController();
            }
            else
            {
                input = MoblieController();
            }

            player.SetMoveInput(input);

        }
    }

    private Vector2 MoblieController()
    {

        switch (moveType)
        {
            case MoveControlType.Joystick:
                return new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
            case MoveControlType.Dpad:
                return new Vector2(dpad.Horizontal, dpad.Vertical);
            case MoveControlType.Keyboard:
                return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        return Vector2.zero; // Default case if no input type matches
    }

    public override void UpdateUIState()
    {
        if (joystick != null) joystick.gameObject.SetActive(false);
        if (dpad != null) dpad.gameObject.SetActive(false);

        switch (moveType)
        {
            case MoveControlType.Joystick:
                if (joystick != null) joystick.gameObject.SetActive(true);
                break;
            case MoveControlType.Dpad:
                if (dpad != null) dpad.gameObject.SetActive(true);
                break;
            case MoveControlType.Keyboard:
                // Không hiện gì cả
                break;
        }
    }
}
