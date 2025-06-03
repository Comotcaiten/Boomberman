using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MysteryBoxItem : Item
{
    [SerializeField] private List<GameObject> items; // List of items to spawn from the mystery box
    [SerializeField] private List<GameObject> enemies; // Danh sách prefab quái có thể spawn
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the sprite renderer for the item
    [SerializeField] private GameObject GameObjectSpriteRenderer; // Reference to the sprite renderer for the item

    private bool isActive = false; // Flag to check if the item is active
    private bool isDone = false; // Flag to check if the item effect is done

    private float time = 0f; // Timer for the item
    private int index = 0; // Index for the item

    void Update()
    {

        if (!isActive) return;
        if (isDone) return; // If the effect is already done, do nothing
        time += Time.deltaTime;
        if (time >= 0.5f)
        {
            time = 0f;
            index++;
            if (index >= items.Count)
            {
                index = 0; // Reset index to loop through the items
            }
            spriteRenderer.sprite = items[index].GetComponent<SpriteRenderer>().sprite; // Update the sprite
        }

    }

    public override IEnumerator Effect()
    {
        // Nếu cả hai đều rỗng thì không làm gì
        if ((items == null || items.Count == 0) && (enemies == null || enemies.Count == 0))
        {
            Debug.LogWarning("No items or enemies available in the mystery box.");
            yield break;
        }

        int totalChance = items.Count + enemies.Count + 1; // +1 là "xui" không có gì
        int randomIndex = Random.Range(0, totalChance);

        yield return new WaitForSeconds(2f);
        isDone = true;

        if (randomIndex < items.Count)
        {
            // Spawn item
            GameObject selectedItem = items[randomIndex];
            Instantiate(selectedItem, transform.position, Quaternion.identity);
        }
        else if (randomIndex < items.Count + enemies.Count)
        {
            // Spawn enemy
            int enemyIndex = randomIndex - items.Count;
            GameObject selectedEnemy = enemies[enemyIndex];
            Instantiate(selectedEnemy, transform.position, Quaternion.identity);
            Debug.Log("Unlucky! An enemy appeared.");
        }
        else
        {
            // Xui thật, không có gì cả
            spriteRenderer.sprite = null;
            Debug.Log("So unlucky, nothing spawned.");
        }


        // DestroyItem();
        gameObject.SetActive(false); // Deactivate the mystery box item
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDone) return; // If the effect is already done, do nothing
        if (isActive) return; // If the item is already active, do nothing
        if (collision.CompareTag("Player") && isActive == false)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player == null)
            {
                Debug.Log("PlayerController not found");
                return;
            }
            // player.StartCoro utine(Effect());

            GetComponent<SpriteRenderer>().enabled = false; // Hide the mystery box sprite
            player.TakeItem(this);
            // DestroyItem();
            isActive = true; // Set the item as active
            GameObjectSpriteRenderer.SetActive(true); // Ensure the sprite renderer is active
        }
    }
}
