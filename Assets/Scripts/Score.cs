using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        AddScore(GameManager.Instance.totalscore);
    }

    public void AddScore(int score)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }

        GameManager.Instance.totalscore += score;
        Debug.Log("Score: " + GameManager.Instance.totalscore.ToString());
        scoreText.text = "Score: " + GameManager.Instance.totalscore.ToString();

        DataManager.SetHighScore(GameManager.Instance.totalscore);
    }
}
