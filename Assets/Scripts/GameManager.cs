using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

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
    public GameObject mysteryBoxPrefab;
    public GameObject portalPrefab;

    private TimeCount timeCount;

    public int levelIndex { get; private set; } = 1;

    // In Level
    private List<GameObject> enemies = new List<GameObject>();

    private bool isGameOver = false;
    public bool isGameWin { get; private set; } = false;

    private GameObject gameOverUI;
    private GameObject gameWinnerUI;

    private GameObject player;

    public int totalscore = 0;

    public float spawnChance = 0.2f; // field

    public float SpawnChance
    {
        get { return spawnChance; }
        set
        {
            spawnChance = value;
            if (spawnChance < 0 || spawnChance > 1)
            {
                spawnChance = 0.15f; // default value
            }
            if (spawnChance >= 0.45f)
            {
                spawnChance = 0.45f; // cap value
            }
        }
    }

    public int itemAmount = 4; // Số lượng item có thể spawn trong màn chơi
    public int spawnItemAmount;
    public int maxItemAmount = 10; // Số lượng tối đa item có thể spawn

    public InputSettings inputSettings;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Instance.inputSettings = new InputSettings();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel()
    {
        try
        {
            spawnItemAmount = itemAmount;
            int rows = 13;
            int columns = 31;
            char[,] map = GenerateRandomMap(rows, columns);

            FileLevelLoader.Rows = rows;
            FileLevelLoader.Columns = columns;
            FileLevelLoader.MapLines = new string[rows];
            for (int row = 0; row < rows; row++)
            {
                char[] rowChars = new char[columns];
                for (int col = 0; col < columns; col++)
                {
                    rowChars[col] = map[row, col];
                }
                FileLevelLoader.MapLines[row] = new string(rowChars);
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    char type = map[row, col];
                    Vector3Int tilePos = new Vector3Int(col - 1, -row - 1, 0);
                    Vector3 worldPos = new Vector3(col, -row, 0);

                    switch (type)
                    {
                        case '*':
                            SetTile(destructibleTilemap, brickTile, tilePos);
                            SpawnItemWithBrick(tilePos, worldPos);
                            break;
                        case '#':
                            SetTile(indestructibleTilemap, wallTile, tilePos);
                            break;
                        case 'x':
                            Spawn(portalPrefab, worldPos, "Portal");
                            SetTile(destructibleTilemap, brickTile, tilePos);
                            break;
                        case 'p':
                            SetUpPlayer(playerPrefab, worldPos);
                            break;
                        case '1':
                            Spawn(enemyBalloomPrefab, worldPos, "EnemyBalloom");
                            break;
                        case '2':
                            Spawn(enemyOnealEnemyPrefab, worldPos, "EnemyOneal");
                            break;
                    }
                    SetTile(grassTilemap, grassTile, tilePos);
                }
            }

            Debug.Log($"Random map generated for level {levelIndex}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to generate map: {e.Message}");
        }
    }

    private char[,] GenerateRandomMap(int rows, int columns)
    {
        char[,] map = new char[rows, columns];

        // Khởi tạo toàn bộ bản đồ là cỏ (' ')
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                map[row, col] = ' ';
            }
        }

        // Đặt tường không thể phá hủy (#) ở biên
        for (int row = 0; row < rows; row++)
        {
            map[row, 0] = '#';
            map[row, columns - 1] = '#';
        }
        for (int col = 0; col < columns; col++)
        {
            map[0, col] = '#';
            map[rows - 1, col] = '#';
        }

        // Đặt tường không thể phá hủy (#) ở lưới 2x2
        for (int row = 2; row < rows - 1; row += 2)
        {
            for (int col = 2; col < columns - 1; col += 2)
            {
                map[row, col] = '#';
            }
        }

        // Đặt người chơi (p) ở góc trên bên trái
        map[1, 1] = 'p';
        map[1, 2] = ' ';
        map[2, 1] = ' ';
        map[2, 2] = ' ';

        map[1, 3] = '*';
        map[2, 3] = '*';
        map[3, 3] = '*';
        map[3, 1] = '*';
        map[2, 1] = '*';
        map[3, 1] = '*';

        // Đặt cổng (x) ở vị trí ngẫu nhiên
        Vector2Int portalPos = GetRandomEmptyPosition(map, rows, columns);
        map[portalPos.y, portalPos.x] = 'x';

        // Đặt kẻ thù (1, 2) ở các vị trí ngẫu nhiên
        int enemyCount = Mathf.Min(4 + levelIndex, 8); //Số lượng kẻ thù tăng dần theo mỗi màn chơi tối đa là 8
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2Int enemyPos = GetRandomEmptyPosition(map, rows, columns);
            map[enemyPos.y, enemyPos.x] = (i % 2 == 0) ? '1' : '2';
        }

        // Đặt gạch có thể phá hủy (*) ở các vị trí ngẫu nhiên, trừ vùng an toàn quanh người chơi
        float brickChance = 0.5f;
        for (int row = 1; row < rows - 1; row++)
        {
            for (int col = 1; col < columns - 1; col++)
            {
                // Chỉ đặt gạch ở các ô trống và không nằm trong vùng an toàn (1,2), (2,1), (2,2), (1,3), (2,3), (3,1), (3,2)
                if (map[row, col] == ' ' && 
                    !(row == 1 && col == 2) && 
                    !(row == 2 && col == 1) && 
                    !(row == 2 && col == 2) && 
                    !(row == 1 && col == 3) && 
                    !(row == 2 && col == 3) && 
                    !(row == 3 && col == 1) && 
                    !(row == 3 && col == 2))
                {
                    if (Random.value < brickChance)
                    {
                        map[row, col] = '*';
                    }
                }
            }
        }

        // In bản đồ ra Console để debug
        DebugMap(map, rows, columns);

        return map;
    }

    private Vector2Int GetRandomEmptyPosition(char[,] map, int rows, int columns)
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();
        for (int row = 1; row < rows - 1; row++)
        {
            for (int col = 1; col < columns - 1; col++)
            {
                // Chỉ thêm các ô trống và không nằm trong vùng an toàn (1,2), (2,1), (2,2), (1,3), (2,3), (3,1), (3,2)
                if (map[row, col] == ' ' && 
                    !(row == 1 && col == 2) && 
                    !(row == 2 && col == 1) && 
                    !(row == 2 && col == 2) && 
                    !(row == 1 && col == 3) && 
                    !(row == 2 && col == 3) && 
                    !(row == 3 && col == 1) && 
                    !(row == 3 && col == 2))
                {
                    emptyPositions.Add(new Vector2Int(col, row));
                }
            }
        }

        if (emptyPositions.Count == 0)
        {
            Debug.LogError("No empty positions available for spawning!");
            return new Vector2Int(1, 1);
        }

        int index = Random.Range(0, emptyPositions.Count);
        return emptyPositions[index];
    }

    private void DebugMap(char[,] map, int rows, int columns)
    {
        string mapString = "";
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                mapString += map[row, col];
            }
            mapString += "\n";
        }
        Debug.Log("Generated Map:\n" + mapString);
    }

    public void AssignTilemap(Tilemap destruct, Tilemap indestruct, Tilemap grass)
    {
        destructibleTilemap = destruct;
        indestructibleTilemap = indestruct;
        grassTilemap = grass;
    }

    public void AssignGameUI(GameObject gameOver, GameObject gameWinner)
    {
        gameOverUI = gameOver;
        gameWinnerUI = gameWinner;
    }

    public void AssignPlayer(GameObject playerObj)
    {
        player = playerObj;
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
        if (prefab == null)
        {
            Debug.LogError($"Prefab for {name} is null!");
            return;
        }
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.name = name;
        Debug.Log($"{name} spawned at {pos}");

        if (prefab.CompareTag("Enemy"))
        {
            if (obj != null)
            {
                enemies.Add(obj);
                CircleCollider2D enemyCollider = obj.GetComponent<CircleCollider2D>();
                for (int i = 0; i < enemies.Count; i++)
                {
                    CircleCollider2D otherEnemyCollider = enemies[i].GetComponent<CircleCollider2D>();
                    if (otherEnemyCollider != null && otherEnemyCollider != enemyCollider)
                    {
                        Physics2D.IgnoreCollision(enemyCollider, otherEnemyCollider);
                    }
                }
            }
            else
            {
                Debug.LogError($"Prefab {name} does not have an Enemy component.");
            }
        }
    }

    private void SetUpPlayer(GameObject playerPrefab, Vector3 pos)
    {
        if (player == null)
        {
            player = Instantiate(playerPrefab, pos, Quaternion.identity);
            player.name = "Player";
            Debug.Log("Player spawned");
        }
        else
        {
            player.SetActive(true);
            player.transform.position = pos;
            Debug.Log("Player already exists in the scene.");
        }
    }

    private void SpawnItemWithBrick(Vector3Int tilePos, Vector3 worldPos)
    {
        SetTile(destructibleTilemap, brickTile, tilePos);

        if (Random.value > SpawnChance)
        {
            return;
        }
        if (spawnItemAmount > 0)
        {
            int rand = Random.Range(0, 15);
            GameObject selectedItemPrefab = null;
            switch (rand)
            {
                case 0:
                    selectedItemPrefab = powerupFlamePrefab;
                    break;
                case 1:
                    selectedItemPrefab = powerupBombPrefab;
                    break;
                case 2:
                    selectedItemPrefab = powerupSpeedPrefab;
                    break;
                case 3:
                    selectedItemPrefab = mysteryBoxPrefab;
                    break;
            }
            if (rand < 4)
            {
                spawnItemAmount--;
            }   

            if (selectedItemPrefab != null)
            {
                Spawn(selectedItemPrefab, worldPos, selectedItemPrefab.name);
                Debug.Log($"Item {selectedItemPrefab.name} spawned at {worldPos}");
            }
            else
            {
                Debug.Log("Selected item prefab is null!");
            }
        }
    }

    public void SetLevelIndex(int index)
    {
        if (index < 0)
        {
            Debug.LogError("Invalid level index");
            return;
        }
        levelIndex = index;
        Debug.Log($"Level index set to {levelIndex}");
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");
        StartCoroutine(LoadGameOverUI());

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().FreezeMovement();
        }

        ClearForGameOver();
    }

    public void GameWin()
    {
        if (isGameWin) return;

        isGameWin = true;
        Debug.Log("You Win!");
        StartCoroutine(LoadGameWinnerUI());
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().FreezeMovement();
        }
    }

    public void UpdateEnemyCount()
    {
        enemies.RemoveAll(enemy => enemy.GetComponent<Enemy>().isDead);
        Debug.Log($"Enemies count after update: {enemies.Count}");
    }

    public void ClearForNextLevel()
    {
        isGameOver = false;
        isGameWin = false;
        enemies.Clear();
    }

    public void ClearForResetGame()
    {
        isGameOver = false;
        isGameWin = false;
        enemies.Clear();
        totalscore = 0;
        levelIndex = 1;
        spawnChance = 0.2f;
        itemAmount = 4;

    }

    private void ClearForGameOver()
    {
        totalscore = 0;
        levelIndex = 1;
        itemAmount = 4;
    }

    private IEnumerator LoadGameOverUI()
    {
        yield return new WaitForSeconds(2f);
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            gameWinnerUI.SetActive(false);
        }
        else
        {
            Debug.Log("Game Over UI is not assigned.");
        }
    }

    private IEnumerator LoadGameWinnerUI()
    {
        yield return new WaitForSeconds(2f);
        if (gameWinnerUI != null)
        {
            gameWinnerUI.SetActive(true);
            gameOverUI.SetActive(false);
        }
        else
        {
            Debug.Log("Game Winner UI is not assigned.");
        }
    }

    public void PortalActive()
    {
        UpdateEnemyCount();
        if (player == null)
        {
            Debug.LogError("Player is not assigned. Cannot proceed to the next level.");
            return;
        }

        if (player.GetComponent<PlayerController>().isDead)
        {
            Debug.Log("Player is dead. Cannot proceed to the next level.");
            return;
        }

        if (enemies.Count > 0)
        {
            Debug.Log("Enemies are still present. Cannot proceed to the next level.");
            return;
        }

        SetLevelIndex(levelIndex + 1);
        if (spawnChance < 0.45f)
        {
            timeCount.elapsedTime += 30f; // Tăng thời gian chơi thêm 30 giây
            itemAmount += 1;
            spawnChance += 0.025f;
            if (itemAmount > maxItemAmount)
            {
                itemAmount = maxItemAmount;
            }
        }
        else
        {
            spawnChance = 0.45f;
        }

        if (levelIndex > 10)
        {
            Debug.Log("No more levels. You win the game!");
            GameWin();
            levelIndex = 1;
            return;
        }

        ClearForNextLevel();
        SceneManager.LoadScene(1);
    }
}

[System.Serializable]
public class InputSettings
{
    public MoveInputController.MoveControlType moveControlType = MoveInputController.MoveControlType.Joystick;
    public PlaceBombInputController.PlaceBombControlType placeBombControlType = PlaceBombInputController.PlaceBombControlType.Button;

    public InputSettings()
    {
        if (PlatformUtils.IsMobilePlatform())
        {
            Debug.Log("Mobile platform detected. Setting default input types.");
            moveControlType = MoveInputController.MoveControlType.Joystick;
            placeBombControlType = PlaceBombInputController.PlaceBombControlType.Button;
        }
        else
        {
            Debug.Log("Non-mobile platform detected. Setting default input types.");
            moveControlType = MoveInputController.MoveControlType.Keyboard;
            placeBombControlType = PlaceBombInputController.PlaceBombControlType.Keyboard;
        }
    }
}

// public static class FileLevelLoader
// {
//     public static int Rows { get; set; }
//     public static int Columns { get; set; }
//     public static string[] MapLines { get; set; }

//     public static void LoadFromText(string text)
//     {
//         // Không cần triển khai vì sử dụng map ngẫu nhiên
//     }
// }