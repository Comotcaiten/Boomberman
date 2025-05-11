using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainUIHandler : MonoBehaviour
{
    
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
