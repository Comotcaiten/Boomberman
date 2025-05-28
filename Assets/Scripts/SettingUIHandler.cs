using UnityEngine;
using UnityEngine.UI;

public class SettingUIHandler : MonoBehaviour
{
    [SerializeField] private Button buttonSetting;
    [SerializeField] private GameObject settingPanel;

    [SerializeField] private SettingGameController settingGameController;

    private void Start()
    {
        // Initialize the setting panel to be inactive
        settingPanel.SetActive(false);

        // Add listener to the button to toggle the setting panel
        buttonSetting.onClick.AddListener(OnSettingButtonClicked);

        settingGameController = settingPanel.GetComponent<SettingGameController>();
        if (settingGameController == null)
        {
            Debug.LogError("SettingGameController component not found on the setting panel.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the setting panel when Escape is pressed
            OnSettingButtonClicked();
        }
    }

    public void OnSettingButtonClicked()
    {
        if (settingPanel.activeSelf)
        {
            settingPanel.SetActive(false);
        }
        else
        {
            settingPanel.SetActive(true);
        }
    }
    
}
