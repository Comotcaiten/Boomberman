using TMPro;
using UnityEngine;

public class EnmiesCount : MonoBehaviour
{

    public TextMeshProUGUI enemiesCountText;
    public void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemyCount = enemies.Length;

        // Update the UI or perform any other actions with the enemy count
        enemiesCountText.text = "Enemies: " + enemyCount.ToString();
    }
}
