using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{

    public float timeDestory;

    public Tilemap tileDestruction;

    public GameObject flameStartPrefab;
    public GameObject flameMiddlePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfter());

        tileDestruction = GameObject.Find("Destruction").GetComponent<Tilemap>();
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(2.0f);

        Vector3Int center = tileDestruction.WorldToCell(transform.position);
        int range = 2; // bán kính nổ (2 ô mỗi hướng)

        // Phá tile ở vị trí tâm
        tileDestruction.SetTile(center, null);
        Debug.Log("Destroyed tile at: " + center);

        // Hướng trái và phải (x)
        for (int dx = 1; dx <= range; dx++)
        {
            Vector3Int posRight = new Vector3Int(center.x + dx, center.y, 0);
            Vector3Int posLeft = new Vector3Int(center.x - dx, center.y, 0);

            tileDestruction.SetTile(posRight, null);
            tileDestruction.SetTile(posLeft, null);

            Debug.Log("Destroyed tile at: " + posRight);
            Debug.Log("Destroyed tile at: " + posLeft);
        }

        // Hướng lên và xuống (y)
        for (int dy = 1; dy <= range; dy++)
        {
            Vector3Int posUp = new Vector3Int(center.x, center.y + dy, 0);
            Vector3Int posDown = new Vector3Int(center.x, center.y - dy, 0);

            tileDestruction.SetTile(posUp, null);
            tileDestruction.SetTile(posDown, null);

            Debug.Log("Destroyed tile at: " + posUp);
            Debug.Log("Destroyed tile at: " + posDown);
        }

        Destroy(gameObject);

        // Hiệu ứng nổ ở quả bom - start
        ExplodeAt(transform.position, flameStartPrefab);
    }

    void ExplodeAt(Vector2 position, GameObject flamePrefab)
    {
        Vector3Int cellPos = tileDestruction.WorldToCell(position);
        tileDestruction.SetTile(cellPos, null);

        Instantiate(flamePrefab, position, Quaternion.identity);
    }
}
