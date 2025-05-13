using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tiles")]
    public Tile wallTile;
    public Tile brickTile;
    public Tile grassTile;

    [Header("Tilemaps")]
    private Tilemap destructibleTilemap;
    private Tilemap indestructibleTilemap;
    private Tilemap grassTilemap;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyBalloomPrefab;
    public GameObject powerupFlamePrefab;
    public GameObject powerupBombPrefab;
    public GameObject powerupSpeedPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(string path)
    {
        Debug.Log($"Loading level from {path}");
        try
        {
            FileLevelLoader.Load(path);

            // Cache tilemaps
            // destructibleTilemap = GameObject.Find("Destruction").GetComponent<Tilemap>();
            // indestructibleTilemap = GameObject.Find("Indestruction").GetComponent<Tilemap>();
            // grassTilemap = GameObject.Find("Floor").GetComponent<Tilemap>();

            for (int row = 0; row < FileLevelLoader.Rows; row++)
            {
                for (int col = 0; col < FileLevelLoader.Columns; col++)
                {
                    char type = FileLevelLoader.MapLines[row][col];
                    Vector3Int tilePos = new Vector3Int(col - 1, -row - 1, 0);
                    Vector3 worldPos = new Vector3(col, -row, 0);

                    switch (type)
                    {
                        case '*':
                            SetTile(destructibleTilemap, brickTile, tilePos);
                            break;
                        case '#':
                            SetTile(indestructibleTilemap, wallTile, tilePos);
                            break;
                        case 'p':
                            Spawn(playerPrefab, worldPos, "Player");
                            break;
                        case '1':
                            Spawn(enemyBalloomPrefab, worldPos, "EnemyBalloom");
                            break;
                        case 'b':
                            SpawnItemWithBrick(powerupBombPrefab, tilePos, worldPos);
                            break;
                        case 'f':
                            SpawnItemWithBrick(powerupFlamePrefab, tilePos, worldPos);
                            break;
                        case 's':
                            SpawnItemWithBrick(powerupSpeedPrefab, tilePos, worldPos);
                            break;
                    }

                    SetTile(grassTilemap, grassTile, tilePos); // luôn vẽ grass dưới cùng
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load level: {e.Message}");
        }
    }

    public void AssignTilemap(Tilemap destruct, Tilemap indestruct, Tilemap grass) {
        destructibleTilemap = destruct;
        indestructibleTilemap = indestruct;
        grassTilemap = grass;
    }

    private void SetTile(Tilemap tilemap, Tile tile, Vector3Int pos)
    {
        tilemap.SetTile(pos, tile);
    }

    private void Spawn(GameObject prefab, Vector3 pos, string name)
    {
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.name = name;
        Debug.Log($"{name} spawned at {pos}");
    }

    private void SpawnItemWithBrick(GameObject itemPrefab, Vector3Int tilePos, Vector3 worldPos)
    {
        SetTile(destructibleTilemap, brickTile, tilePos);
        Spawn(itemPrefab, worldPos, itemPrefab.name);
    }
}
