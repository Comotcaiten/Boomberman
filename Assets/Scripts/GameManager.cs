using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

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


    // In Level
    private List<GameObject> enemies = new List<GameObject>();

    private bool isGameOver = false;
    public bool isGameWin { get; private set; } = false;

    private GameObject gameOverUI;
    private GameObject gameWinnerUI;

    [SerializeField] private List<TextAsset> levelFiles;

    private GameObject player;
    private GameObject scoreText;

    public int totalscore = 0;

    public float spawnChance; // field

    private float SpawnChance = 0.2f; // 20% chance to spawn an item when a brick is destroyed

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
            // C2
            if (levelIndex - 1 < 0 || levelIndex - 1 >= levelFiles.Count)
            {
                Debug.LogError("Invalid level index or level file not assigned.");
                return;
            }

            TextAsset levelFile = levelFiles[levelIndex - 1];
            FileLevelLoader.LoadFromText(levelFile.text);


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
                            // Spawn(playerPrefab, worldPos, "Player");
                            SetUpPlayer(playerPrefab, worldPos);
                            break;
                        case '1':
                            Spawn(enemyBalloomPrefab, worldPos, "EnemyBalloom");
                            break;
                        case '2':
                            Spawn(enemyOnealEnemyPrefab, worldPos, "EnemyOneal");
                            // Spawn(enemyBalloomPrefab, worldPos, "EnemyBalloom");
                            break;
                            // case 'b':
                            //     SpawnItemWithBrick(tilePos, worldPos);
                            //     break;
                            // case 'f':
                            //     SpawnItemWithBrick(tilePos, worldPos);
                            //     break;
                            // case 's':
                            //     SpawnItemWithBrick(tilePos, worldPos);
                            //     break;

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
        var obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.name = name;
        // Debug.Log($"{name} spawned at {pos}");

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

        // Tỷ lệ spawn item, ví dụ 20% (có thể điều chỉnh)
        if (Random.value > SpawnChance) // Nếu Random.value > 0.2, tức 80% không spawn
        {
            return;
        }

        // Chọn ngẫu nhiên một loại item
        int rand = Random.Range(0, 3);
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
        }

        if (selectedItemPrefab != null)
        {
            Spawn(selectedItemPrefab, worldPos, selectedItemPrefab.name);
            Debug.Log($"Item {selectedItemPrefab.name} spawned at {worldPos}");
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
        // Handle game over logic here (e.g., show game over screen, reset level, etc.)

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

        // Handle game win logic here (e.g., show win screen, load next level, etc.)
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

    private void ClearForGameOver()
    {
        totalscore = 0;
        levelIndex = 1;
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
        // Đầu tiên kiểm tra player và enemies
        // Cập nhập số lượng kẻ thù còn lại trước
        UpdateEnemyCount();
        // Nếu player không chết và không còn kẻ thù nào thì mới cho phép đi qua cổng
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

        // Nếu tất cả điều kiện trên đều thỏa mãn thì tăng level index và load level mới
        SetLevelIndex(levelIndex + 1);
        if (spawnChance < 0.45f)
        {
            spawnChance += 0.025f; // Increase spawn chance for the next level
        }
        else
        {
            spawnChance = 0.45f; // Cap the spawn chance at 45%
        }
        // itemAmount += 1; // Increase item amount for the next level



        // Check if the level index is valid

        // C2
        if (levelIndex > levelFiles.Count)
        {
            Debug.Log("No more levels. You win the game!");
            GameWin();
            levelIndex = 1;
            return;
        }

        // Dọn dẹp các đối tượng cũ cần cho level trước khi load level mới
        ClearForNextLevel();

        // Load the new level
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
        // Default constructor
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
