using System.Collections;
using UnityEngine;

public class SpeedItem : Item
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float duration = 5.0f;
    PlayerController player;

    protected override IEnumerator Effect()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.speed += speed;
            yield return new WaitForSeconds(duration);
            // player.speed -= speed;
        }
        else
        {
            Debug.Log("Player not found");
        }
    }
}
