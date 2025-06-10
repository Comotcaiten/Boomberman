using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tileHighScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.PlaySound(SoundType.MUSIC);
        tileHighScore.text = "High Score: " + DataManager.GetHighScore().ToString();
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
