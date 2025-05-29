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

        var moveIndex = moveControlOptions.IndexOf(DataManager.GetMoveControlType().ToString());
        moveDropdown.value = (int)moveIndex;
        // moveDropdown.value = moveIndex >= 0 ? moveIndex : 0;
        Debug.Log("Move DropDown: " + moveDropdown.value);
        moveDropdown.onValueChanged.AddListener(OnMoveTypeChanged);

        placeBombDropdown.ClearOptions();
        placeBombDropdown.AddOptions(placeBombControlOptions);

        var bombIndex = placeBombControlOptions.IndexOf(DataManager.GetBombControlType().ToString());
        // placeBombDropdown.value = bombIndex >= 0 ? bombIndex : 0;
        placeBombDropdown.value = (int)bombIndex;
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
        var selectedType = (MoveInputController.MoveControlType)index;
        DataManager.SetMoveControlType(selectedType);

        moveInputController.moveType = selectedType;
        moveInputController.UpdateUIState();

        Debug.Log("Move control changed to: " + moveInputController.moveType);
    }


    private void OnPlaceBombControlChanged(int index)
    {
        var selectedType = (PlaceBombInputController.PlaceBombControlType)index;
        DataManager.SetBombControlType(selectedType);

        placeBombInputController.placeBombType = selectedType;
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
