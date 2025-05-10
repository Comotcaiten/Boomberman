using System.Collections;
using UnityEngine;

public class SpeedItem : Item
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float duration = 5.0f;
    PlayerController player;

    protected override void Effect()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartCoroutine(SpeedUp());
        }
        else
        {
            Debug.LogWarning("Player not found");
        }
    }

    IEnumerator SpeedUp()
    {
        if (player != null)
        {
            player.speed += speed;
            yield return new WaitForSeconds(duration);
            player.speed -= speed;
        }
    }
}
