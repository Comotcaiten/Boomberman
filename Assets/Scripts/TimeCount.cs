using UnityEngine;
using TMPro;

public class TimeCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime = 300f;
    private bool isRunning = true;
    private PlayerController player;

    void Start()
    {
        isRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        // Find player if not yet assigned
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
            if (player == null)
            {
                Debug.LogWarning("PlayerController not found in the scene yet!");
                return; // Skip until player is found
            }
            else
            {
                Debug.Log("PlayerController found successfully.");
            }
        }

        if (isRunning)
        {
            elapsedTime -= Time.deltaTime;
            UpdateTimerDisplay();
            if (elapsedTime <= 0f || player.isDead)
            {
                elapsedTime = 0f;
                isRunning = false;
                GameObject.Find("UITime").SetActive(false); // Hide the timer UI
                player.SetIsDead(true); // Mark player as dead
                Debug.Log("Time's up! Player has died.");
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.FloorToInt(elapsedTime).ToString();
        }
        else
        {
            Debug.LogWarning("TimerText is not assigned in the Inspector.");
        }
    }
}