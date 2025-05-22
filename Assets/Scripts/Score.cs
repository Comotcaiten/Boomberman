using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        addScore(GameManager.Instance.totalscore);
    }

    public void addScore(int score)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }

        GameManager.Instance.totalscore += score;
        Debug.Log("Score: " + GameManager.Instance.totalscore.ToString());
        scoreText.text = "Score: " + GameManager.Instance.totalscore.ToString();
    }
}
