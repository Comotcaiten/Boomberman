using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager instance is null. Make sure GameManager is initialized.");
            return;
        }

        GameManager.Instance.PortalActive();
    }

}
