using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    
    public float timeDestory;

    public Tilemap tileDestruction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfter());

        tileDestruction = GameObject.Find("Destruction").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyAfter() {
        yield return new WaitForSeconds(5.0f);

        // float posX = transform.position.x;
        // float posY = transform.position.y;

        // for (float i = (posX - 1); i < (posX + 2); i++) {
        //     for (float j = (posY - 1); j < (posY + 2); j++) {
        //         // Debug.Log($"pos: {i}, {j}");

        //         Vector3Int pos = Vector3Int.zero;
        //         pos.x = Mathf.RoundToInt(i);
        //         pos.y = Mathf.RoundToInt(j);

        //         Debug.Log($"pos: {pos}");

        //         tileDestruction.SetTile(pos, null);
        //     }
        // }


    // Vector3Int bombCellPos = tileDestruction.WorldToCell(transform.position);

    // for (int dx = -1; dx <= 1; dx++) {
    //     for (int dy = -1; dy <= 1; dy++) {
    //         Vector3Int pos = new Vector3Int(bombCellPos.x + dx, bombCellPos.y + dy, 0);
    //         Debug.Log($"Destroying tile at: {pos}");

    //         tileDestruction.SetTile(pos, null);
    //     }
    // }

        Destroy(gameObject);
    }
}   
