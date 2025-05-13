using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.IO;

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
    public GameObject enemyOnealEnemyPrefab;
    public GameObject powerupFlamePrefab;
    public GameObject powerupBombPrefab;
    public GameObject powerupSpeedPrefab;
    public GameObject portalPrefab;

    public int levelIndex { get; private set; } = 1;

    private string path { get { return Application.dataPath + "/Levels/Level" + levelIndex + ".txt"; } }

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

    public void LoadLevel()
    {
        Debug.Log($"Loading level from {path}");
        try
        {
            FileLevelLoader.Load(path);

            for (int row = 0; row < FileLevelLoader.Rows; row++)
            {
                for (int col = 0; col < FileLevelLoader.Columns; col++)
                {
                    char type = FileLevelLoader.MapLines[row][col];

                    // do để offset của Grid Tilemap là 0.5 nên phải để trừ 1 (làm tròn 0.5)
                    Vector3Int tilePos = new Vector3Int(col - 1, -row - 1, 0);

                    // Cái này là để cho các đối tượng spawn ra có vị trí đúng
                    Vector3 worldPos = new Vector3(col, -row, 0);

                    switch (type)
                    {
                        case '*':
                            SetTile(destructibleTilemap, brickTile, tilePos);
                            break;
                        case '#':
                            SetTile(indestructibleTilemap, wallTile, tilePos);
                            break;
                        case 'x':
                            Spawn(portalPrefab, worldPos, "Portal");
                            SetTile(destructibleTilemap, brickTile, tilePos);
                            break;
                        case 'p':
                            Spawn(playerPrefab, worldPos, "Player");
                            break;
                        case '1':
                            Spawn(enemyBalloomPrefab, worldPos, "EnemyBalloom");
                            break;
                        case '2':
                            Spawn(enemyOnealEnemyPrefab, worldPos, "EnemyOneal");
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

    public void AssignTilemap(Tilemap destruct, Tilemap indestruct, Tilemap grass)
    {
        destructibleTilemap = destruct;
        indestructibleTilemap = indestruct;
        grassTilemap = grass;
    }

    private void SetTile(Tilemap tilemap, Tile tile, Vector3Int pos)
    {

        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned.");
            return;
        }
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

    public void SetLevelIndex(int index)
    {
        if (index < 0)
        {
            Debug.LogError("Invalid level index. Must be between 1 and 3.");
            return;
        }
        levelIndex = index;

        Debug.Log($"Level index set to {levelIndex}");

        // Check if the level index is valid

        if (!File.Exists(path))
        {
            Debug.Log($"Level file not found: {path}");
            SceneManager.LoadScene(0); // Load the menu scene (index 0)
            levelIndex = 0;
            return;
        }

        // Load the new level
        SceneManager.LoadScene(1);
    }

    public void LoadTileMMMM() {
       Tilemap a = GameObject.Find("Tilemap").GetComponent<Tilemap>();
       a.SetTile(new Vector3Int(0, 0, 0), wallTile);
    }
}
