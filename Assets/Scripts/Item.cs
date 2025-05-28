using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public abstract IEnumerator Effect();

    protected void DestroyItem() {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player == null)
            {
                Debug.Log("PlayerController not found");
                return;
            }
            // player.StartCoro utine(Effect());

            player.TakeItem(this);
            DestroyItem();
        }
    }

}

