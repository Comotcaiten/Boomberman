using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic; // <- Thêm để sử dụng List<GameObject>

public class RandomItem : MonoBehaviour
{
    public Tilemap destructableTilemap;
    public GameObject destructionEffectPrefab;

    // === PHẦN THÊM VÀO: Danh sách item có thể rơi và xác suất rơi ===
    [Header("Item Drop Settings")]
    public List<GameObject> itemPrefabs;       // Danh sách prefab item có thể rơi
    [Range(0f, 1f)]
    public float dropChance = 0.3f;             // Xác suất rơi item (30%)

    private void Start()
    {
        if (destructableTilemap == null)
            destructableTilemap = GetComponent<Tilemap>();
    }

    // Hàm gọi từ vụ nổ
    public void DestroyTileAtWorldPosition(Vector3 worldPosition)
    {
        Vector3Int cellPos = destructableTilemap.WorldToCell(worldPosition);
        TileBase tile = destructableTilemap.GetTile(cellPos);

        if (tile != null)
        {
            // Xóa tile
            destructableTilemap.SetTile(cellPos, null);
            Debug.Log("Destroyed tile at: " + cellPos);

            // Hiệu ứng phá hủy
            if (destructionEffectPrefab != null)
            {
                Vector3 spawnPos = destructableTilemap.GetCellCenterWorld(cellPos) + new Vector3(0, 0, -0.1f);
                GameObject effect = Instantiate(destructionEffectPrefab, spawnPos, Quaternion.identity);

                // Tự động hủy sau khi animation kết thúc
                Animator animator = effect.GetComponent<Animator>();
                if (animator != null)
                {
                    AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
                    if (clips.Length > 0)
                    {
                        float duration = clips[0].clip.length;
                        Destroy(effect, duration);
                    }
                    else
                    {
                        Destroy(effect, 1f); // fallback nếu không có animation
                    }
                }
                else
                {
                    Destroy(effect, 1f);
                }
            }

            // === PHẦN THÊM VÀO: Xử lý rơi item ngẫu nhiên ===
            if (itemPrefabs != null && itemPrefabs.Count > 0 && Random.value <= dropChance)
            {
                int randomIndex = Random.Range(0, itemPrefabs.Count); // chọn item ngẫu nhiên từ danh sách
                GameObject itemToSpawn = itemPrefabs[randomIndex];
                if (itemToSpawn != null)
                {
                    // Tạo item tại vị trí tile bị phá
                    Instantiate(itemToSpawn, destructableTilemap.GetCellCenterWorld(cellPos), Quaternion.identity);
                }
            }
        }
    }
}
