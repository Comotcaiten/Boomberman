using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject mainCamera;
    void Start()
    {
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
        SceneManager.LoadScene(1); // Load the main scene (index 1)
    }

    private void LoadLevel()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("GameManager instance is null. Make sure GameManager is initialized.");
            return;
        }
        
        GameManager.Instance.ClearLevel();

        GameManager.Instance.AssignTilemap(
            GameObject.Find("Destruction").GetComponent<Tilemap>(),
            GameObject.Find("Indestruction").GetComponent<Tilemap>(),
            GameObject.Find("Floor").GetComponent<Tilemap>()
        );

        GameManager.Instance.AssignGameOverUI(gameOverUI);
        gameOverUI.SetActive(false);

        mainCamera.AddComponent<CameraFollow>().player = GameObject.FindGameObjectWithTag("Player");
        mainCamera.GetComponent<CameraFollow>().offset = new Vector3(0, 0, -10);
        mainCamera.GetComponent<CameraFollow>().smoothSpeed = 0.125f;
        mainCamera.GetComponent<CameraFollow>().enabled = true;

        GameManager.Instance.LoadLevel();
    }
}
