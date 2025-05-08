using UnityEngine;

public class BallonEnemy: Enemy
{
    private float directionChangeInterval = 5.0f;
    private float timer;

    protected override void Start() {
        
        base.Start();
        timer = directionChangeInterval;
        Change();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            Debug.Log($"Đổi hướng");

            timer = directionChangeInterval;
            Change();
        }
    }

    protected override void Change()
    {
        int path = Random.Range(0,4);

        switch (path) {
            case 0: 
                moveDir = Vector2.up;
                Debug.Log($"Đổi hướng: Lên");
                break;
            case 1:
                moveDir = Vector2.down;
                Debug.Log($"Đổi hướng: Xuống");
                break;
            case 2: 
                moveDir = Vector2.right;
                Debug.Log($"Đổi hướng: Phải");  
                break;
            case 3:
                moveDir = Vector2.left; 
                Debug.Log($"Đổi hướng: Trái");
                break;
        }
    }
}