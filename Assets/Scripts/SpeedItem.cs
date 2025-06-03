using System.Collections;
using UnityEngine;

public class SpeedItem : Item
{
    [SerializeField] private float speed = 2.0f;
    private float duration = 5.0f;
    PlayerController player;

    public override IEnumerator Effect()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.UpdateSpeed(player.speed + speed); // Assuming there's a method to update the player's speed
            yield return new WaitForSeconds(duration);
            // player.speed -= speed;
        }
        else
        {
            Debug.Log("Player not found");
        }
    }
}
