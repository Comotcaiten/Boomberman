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

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;

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
        moveDropdown.value = (int)GameManager.Instance.inputSettings.moveControlType;
        Debug.Log("Move DropDown: " + moveDropdown.value);
        moveDropdown.onValueChanged.AddListener(OnMoveTypeChanged);

        placeBombDropdown.ClearOptions();
        placeBombDropdown.AddOptions(placeBombControlOptions);
        placeBombDropdown.value = (int)GameManager.Instance.inputSettings.placeBombControlType;
        placeBombDropdown.onValueChanged.AddListener(OnPlaceBombControlChanged);

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        musicVolumeSlider.value = SoundManager.Instance.musicVolume;
        musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("F0") + "%"; // Update the text to show percentage

        sfxVolumeSlider.value = SoundManager.Instance.sfxVolume;
        sfxVolumeText.text = (sfxVolumeSlider.value * 100).ToString("F0") + "%"; // Update the text to show percentage
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

    public void SetMusicVolume(float value)
    {
        SoundManager.SetMusicVolume(value);
        musicVolumeText.text = (value * 100).ToString("F0") + "%"; // Update the text to show percentage
    }

    public void SetSFXVolume(float value)
    {
        SoundManager.SetSFXVolume(value);
        sfxVolumeText.text = (value * 100).ToString("F0") + "%"; // Update the text to show percentage
    }

}
