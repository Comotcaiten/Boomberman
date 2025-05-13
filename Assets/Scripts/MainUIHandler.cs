using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MainUIHandler : MonoBehaviour
{
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
        GameManager.Instance.AssignTilemap(
            GameObject.Find("Destruction").GetComponent<Tilemap>(),
            GameObject.Find("Indestruction").GetComponent<Tilemap>(),
            GameObject.Find("Floor").GetComponent<Tilemap>()
        );

        GameManager.Instance.LoadLevel(Application.dataPath + "/Levels/Level2.txt");
    }
}
