using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingGameController : MonoBehaviour
{
    public MoveInputController moveInputController;
    public PlaceBombInputController placeBombInputController;

    public TMP_Dropdown moveDropdown;
    public TMP_Dropdown placeBombDropdown;
    
    List<string> moveControlOptions = new List<string> { "Joystick", "Dpad", "Keyboard" };
    List<string> placeBombControlOptions = new List<string> { "Button", "Area", "Keyboard" };

    private void Start()
    {
        if (PlatformUtils.IsMobilePlatform())
        {
            moveControlOptions.Remove("Keyboard");
            placeBombControlOptions.Remove("Keyboard");
        }

        // Initialize dropdowns
        moveDropdown.ClearOptions();
        moveDropdown.AddOptions(moveControlOptions);
        moveDropdown.value = (int)moveInputController.moveType;
        moveDropdown.onValueChanged.AddListener(OnMoveTypeChanged);

        placeBombDropdown.ClearOptions();
        placeBombDropdown.AddOptions(placeBombControlOptions);
        placeBombDropdown.value = (int)placeBombInputController.placeBombType;
        placeBombDropdown.onValueChanged.AddListener(OnPlaceBombControlChanged);
    }

    private void OnMoveTypeChanged(int index)
    {
        GameManager.Instance.inputSettings.moveControlType = (MoveInputController.MoveControlType)index;
        moveInputController.moveType = GameManager.Instance.inputSettings.moveControlType;
        moveInputController.UpdateUIState();
        Debug.Log("Move control changed to: " + moveInputController.moveType);
    }


    private void OnPlaceBombControlChanged(int index)
    {
        GameManager.Instance.inputSettings.placeBombControlType = (PlaceBombInputController.PlaceBombControlType)index;
        placeBombInputController.placeBombType = GameManager.Instance.inputSettings.placeBombControlType;
        placeBombInputController.UpdateUIState();
        Debug.Log("Place bomb control changed to: " + placeBombInputController.placeBombType);
    }

}
