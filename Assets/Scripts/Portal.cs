using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetLevelIndex(GameManager.Instance.levelIndex + 1);
            }
            else
            {
                Debug.Log("GameManager instance is null. Make sure GameManager is initialized.");
            }
        }
    }
}
