using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuUIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.PlaySound(SoundType.MUSIC);
    }


    public void LoadMainScene() {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

}
