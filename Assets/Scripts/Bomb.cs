using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    private CircleCollider2D bombCollider;
    private CircleCollider2D bomberCollider;

    public float timeDestory = 2.0f;

    public Tilemap tileDestruction;

    public GameObject flameStartPrefab;
    public GameObject flameMiddlePrefab;

    private bool playerInside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileDestruction = GameObject.Find("Destruction").GetComponent<Tilemap>();

        bombCollider = GetComponent<CircleCollider2D>();
        bomberCollider = GameObject.Find("Player").GetComponent<CircleCollider2D>();

        // Bỏ qua va chạm ban đầu
        Physics2D.IgnoreCollision(bombCollider, bomberCollider, true);
        playerInside = true;

        StartCoroutine(DestroyAfter());
    }

    private void Update() {

        // Giả sử khi đặt bomber đặt bomb thì vị trí bomber == bomb == 0
        // => Đi ra thì kích hoạt va chạm
        if (!playerInside) return;
        if (Vector2.Distance(transform.position, bomberCollider.transform.position) > 0.5f)
        {
            playerInside = false;
            Physics2D.IgnoreCollision(bombCollider, bomberCollider, false); // Kích hoạt lại va chạm
        }
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(timeDestory);

        Debug.Log("DestroyAfter");

        Vector3Int center = tileDestruction.WorldToCell(transform.position);
        int range = 1   ; // bán kính nổ (2 ô mỗi hướng)

        // Nổ phần Up - Down
        Explode(range, center, Vector3Int.up);
        Explode(range, center, Vector3Int.down);

        // Nổ phần Left - Right
        Explode(range, center, Vector3Int.left);
        Explode(range, center, Vector3Int.right);


        Destroy(gameObject);

        // Hiệu ứng nổ ở quả bom - start
        ExplodeEffAt(transform.position, flameStartPrefab);
    }

    void ExplodeEffAt(Vector2 position, GameObject flamePrefab)
    {
        Vector3Int cellPos = tileDestruction.WorldToCell(position);
        tileDestruction.SetTile(cellPos, null);

        Instantiate(flamePrefab, position, Quaternion.identity);
    }

    void Explode(int range, Vector3Int dir, Vector3Int path) 
    {
        for (int i = 1; i <= range; i++)
        {
            Vector3Int linePos = dir + (path * i);

            tileDestruction.SetTile(linePos, null);
        }
    }
}
