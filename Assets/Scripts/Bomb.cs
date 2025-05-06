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

        Debug.Log("DestroyAfter");

        // Vector3Int center = tileDestruction.WorldToCell(transform.position);
        // int range = 2; // bán kính nổ (2 ô mỗi hướng)

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
