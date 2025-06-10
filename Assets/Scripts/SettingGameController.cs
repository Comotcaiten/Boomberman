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

    public GameObject wrapperUIController;
    public GameObject wrapperUISound;

    public Button buttonSound;
    public Button buttonControls;

    public Color selectedColor = new Color(0.2f, 0.6f, 1f, 1f);
    public Color unselectedColor = new Color(0.8f, 0.8f, 0.8f, 1f);

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

        // First ui setting is sound settings
        wrapperUIController.SetActive(false);
        wrapperUISound.SetActive(true);
        // Set the colors of the buttons
        SetColorSoundAndControls(true);
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

    public void OnButtonSound()
    {
        // Switch to sound settings
        wrapperUIController.SetActive(false);
        wrapperUISound.SetActive(true);
        // Set the colors of the buttons
        SetColorSoundAndControls(true);
    }

    public void OnButtonControls()
    {
        wrapperUIController.SetActive(true);
        wrapperUISound.SetActive(false);
        // Set the colors of the buttons
        SetColorSoundAndControls(false);
    }

    public void SetColorSoundAndControls(bool isSoundSelected)
    {
        if (isSoundSelected)
        {
            buttonControls.GetComponent<Image>().color = unselectedColor;
            buttonSound.GetComponent<Image>().color = selectedColor;
        }
        else
        {
            buttonControls.GetComponent<Image>().color = selectedColor;
            buttonSound.GetComponent<Image>().color = unselectedColor;
        }
    }
}
