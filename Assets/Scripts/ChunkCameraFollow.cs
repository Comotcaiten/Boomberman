using UnityEngine;

public class ChunkCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 chunkSize = new Vector2(10, 8); // Kích thước mỗi vùng camera bao quát
    public Vector3 offset;

    void LateUpdate()
    {
        if (player == null) return;

        int chunkX = Mathf.FloorToInt(player.position.x / chunkSize.x);
        int chunkY = Mathf.FloorToInt(player.position.y / chunkSize.y);

        Vector3 targetPos = new Vector3(
            chunkX * chunkSize.x + chunkSize.x / 2,
            chunkY * chunkSize.y + chunkSize.y / 2,
            transform.position.z
        );

        transform.position = targetPos + offset;
    }
}
