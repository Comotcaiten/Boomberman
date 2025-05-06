using UnityEngine;
using UnityEngine.Tilemaps;

public class TileClickDestroyer : MonoBehaviour
{
    public Tilemap tilemap;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = chuột trái
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            if (tilemap.HasTile(cellPosition))
            {
                tilemap.SetTile(cellPosition, null);
                Debug.Log("Tile destroyed at: " + cellPosition);
            }
            else
            {
                Debug.Log("No tile at: " + cellPosition);
            }
        }
    }
}
