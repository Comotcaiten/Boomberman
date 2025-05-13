using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MainUIHandler : MonoBehaviour
{
    public void Awake()
    {
        // GameManager.Instance.LoadLevel(Application.dataPath + "/Levels/Level1.txt");
    }

    void Start()
    {
        GameManager.Instance.AssignTilemap(
            GameObject.Find("Destruction").GetComponent<Tilemap>(),
            GameObject.Find("Indestruction").GetComponent<Tilemap>(),
            GameObject.Find("Floor").GetComponent<Tilemap>()
        );

        GameManager.Instance.LoadLevel(Application.dataPath + "/Levels/Level2.txt");
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
}
