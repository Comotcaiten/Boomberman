using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.UI;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinnerUI;
    [SerializeField] private GameObject levelLoadUI;
    [SerializeField] private GameObject UIMoblie;
    [SerializeField] private Button buttonAreaPlaceBomb;
    [SerializeField] private Button buttonPlaceBomb;
    [SerializeField] private JoystickController joystickController;
    [SerializeField] private DpadController dpadController;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject player;

    private CameraFollow camFollow;
    void Start()
    {
        // #if UNITY_ANDROID || UNITY_IOS
        //         UIMoblie.SetActive(true);
        //         Debug.Log("Mobile UI is active");
        // #else
        //     UIMoblie.SetActive(false);
        //     Debug.Log("Mobile UI is inactive");
        // #endif
        if (IsMobilePlatform())
        {
            UIMoblie.SetActive(true);
        }
        else
        {
            UIMoblie.SetActive(false);
        }

        StartCoroutine(LoadLevelLoadingUI());
        LoadLevel();
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void OnBackMenuButtonClicked()
    {
        SceneManager.LoadScene(0); // Load the menu scene (index 0)
    }

    public void OnRestartButtonClicked()
    {
        if (isGameManagerNull())
        {
            SceneManager.LoadScene(0); // Load the menu scene (index 0)
            return;
        }
        GameManager.Instance.ClearLevel();
        SceneManager.LoadScene(1); // Load the main scene (index 1)
    }

    public void OnNetxLevelButtonClicked()
    {
        GameManager.Instance.LoadLevel(); // Load the next level
    }

    private void LoadLevel()
    {
        if (isGameManagerNull()) return;

        GameManager.Instance.ClearLevel();

        GameManager.Instance.AssignTilemap(
            GameObject.Find("Destruction").GetComponent<Tilemap>(),
            GameObject.Find("Indestruction").GetComponent<Tilemap>(),
            GameObject.Find("Floor").GetComponent<Tilemap>()
        );

        GameManager.Instance.AssignGameUI(gameOverUI, gameWinnerUI);
        gameOverUI.SetActive(false);
        gameWinnerUI.SetActive(false);

        GameManager.Instance.AssignPlayer(player);

        buttonPlaceBomb.onClick.AddListener(() =>
        {
            player.GetComponent<BombController>().ButtonPlaceBomb();
        });

        buttonAreaPlaceBomb.onClick.AddListener(() =>
        {
            player.GetComponent<BombController>().ButtonPlaceBomb();
        });

        dpadController.player = player.GetComponent<PlayerController>();
        joystickController.player = player.GetComponent<PlayerController>();

        // if (IsMobilePlatform())
        // {
        //     camFollow = mainCamera.AddComponent<CameraFollow>();
        //     camFollow.player = GameObject.FindGameObjectWithTag("Player");
        //     camFollow.offset = new Vector3(0, 0, -20);
        //     camFollow.smoothSpeed = 0.125f;
        // }
        camFollow = mainCamera.AddComponent<CameraFollow>();
        camFollow.player = GameObject.FindGameObjectWithTag("Player");
        camFollow.offset = new Vector3(0, -6, -10);
        camFollow.smoothSpeed = 0.125f;

        GameManager.Instance.LoadLevel();

    }

    IEnumerator LoadLevelLoadingUI()
    {
        levelLoadUI.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        levelLoadUI.SetActive(false);
    }

    bool IsMobilePlatform()
    {
        return Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    bool isGameManagerNull()
    {
        if (GameManager.Instance == null)
        {
            gameOverUI.SetActive(true);
            Debug.Log("GameManager instance is null. Make sure GameManager is initialized.");
            return true;
        }
        return false;
    }
}
