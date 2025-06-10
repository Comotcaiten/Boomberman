using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject levelLoadUI;
    [SerializeField] private GameObject UIMoblie;

    [SerializeField] private MoveInputController moveInputController;
    [SerializeField] private PlaceBombInputController placeBombInputController;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject player;

    private CameraFollow camFollow;
    void Start()
    {
        if (PlatformUtils.IsMobilePlatform())
        {
            UIMoblie.SetActive(true);
        }
        else
        {
            UIMoblie.SetActive(true);
        }

        StartCoroutine(LoadLevelLoadingUI());
        LoadLevel();

        SoundManager.PlaySound(SoundType.MUSIC);
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
        if (!isGameManagerNull())
        {
            GameManager.Instance.ClearForResetGame();
        }
        SceneManager.LoadScene(0); // Load the menu scene (index 0)
    }

    // Reset Game
    public void OnRestartButtonClicked()
    {
        if (isGameManagerNull())
        {
            SceneManager.LoadScene(0); // Load the menu scene (index 0)
            return;
        }
        GameManager.Instance.ClearForResetGame();
        SceneManager.LoadScene(1); // Load the main scene (index 1)
    }

    private void LoadLevel()
    {
        if (isGameManagerNull())
        {
            SceneManager.LoadScene(0); // Load the menu scene (index 0)
            return;
        }

        GameManager.Instance.ClearForNextLevel();

        GameManager.Instance.AssignTilemap(
            GameObject.Find("Destruction").GetComponent<Tilemap>(),
            GameObject.Find("Indestruction").GetComponent<Tilemap>(),
            GameObject.Find("Floor").GetComponent<Tilemap>()
        );

        GameManager.Instance.AssignGameUI(gameOverUI);
        gameOverUI.SetActive(false);

        GameManager.Instance.AssignPlayer(player);

        moveInputController.BindPlayer(player);
        placeBombInputController.BindPlayer(player);

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
