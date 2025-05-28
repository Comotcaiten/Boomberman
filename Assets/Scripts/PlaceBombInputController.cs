using UnityEngine;
using UnityEngine.UI;

public class PlaceBombInputController : InputController
{
    [SerializeField] private Button buttonAreaPlaceBomb;
    [SerializeField] private Button buttonPlaceBomb;

    public enum PlaceBombControlType
    {
        Button,
        Area,
        Keyboard
    }

    public PlaceBombControlType placeBombType;

    protected override void Start()
    {
        if (GameManager.Instance != null)
        {
            placeBombType = GameManager.Instance.inputSettings.placeBombControlType; // Get the place bomb control type from GameManager
            Debug.Log("PlaceBombInputController: Place bomb control type set to " + placeBombType);
            UpdateUIState();
        }
        else
        {
            Debug.Log("IputController-Place: GameManager is not initialized. Please bind the player before using input controls.");
        }
        
    }

    void Update()
    {
        if (player == null && GameManager.Instance == null) return;

        if (!PlatformUtils.IsMobilePlatform()) return;
        if (useControllerMoblieOnly || placeBombType == PlaceBombControlType.Keyboard)
        {
            Debug.Log("Using mobile controller only, but place bomb type is not set to Area. Defaulting to Area input.");
            GameManager.Instance.inputSettings.placeBombControlType = PlaceBombControlType.Area; // Update the setting
            placeBombType = PlaceBombControlType.Area; // Update the local variable
            UpdateUIState(); // Update the UI state
        }
    }

    public override void BindPlayer(GameObject player)
    {
        base.BindPlayer(player);

        if (player == null) return;

        if (buttonAreaPlaceBomb != null)
        {
            buttonAreaPlaceBomb.onClick.AddListener(player.GetComponent<BombController>().ButtonPlaceBomb);
        }
        if (buttonPlaceBomb != null)
        {
            buttonPlaceBomb.onClick.AddListener(player.GetComponent<BombController>().ButtonPlaceBomb);
        }
    }

    public override void UpdateUIState()
    {
        if (buttonAreaPlaceBomb != null) buttonAreaPlaceBomb.gameObject.SetActive(false);
        if (buttonPlaceBomb != null) buttonPlaceBomb.gameObject.SetActive(false);
        switch (placeBombType)
        {

            case PlaceBombControlType.Button:
                if (buttonPlaceBomb != null) buttonPlaceBomb.gameObject.SetActive(true);
                break;
            case PlaceBombControlType.Area:
                if (buttonAreaPlaceBomb != null) buttonAreaPlaceBomb.gameObject.SetActive(true);
                break;
            case PlaceBombControlType.Keyboard:
                // Keyboard input is handled in the BombController, no UI needed
                break;
        }
    }
}
