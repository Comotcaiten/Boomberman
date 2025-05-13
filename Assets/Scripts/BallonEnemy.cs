using System.Collections.Generic;
using UnityEngine;

public class BallonEnemy : Enemy
{
    private float directionChangeInterval = 5.0f;
    private float timer;

    private Vector2[] moveDirs = {
        Vector2.up,
        Vector2.down,
        Vector2.right,
        Vector2.left
    };

    protected override void Start()
    {

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
        // int path = Random.Range(0,moveDirs.Length);
        // moveDir = moveDirs[path];

        List<Vector2> validDirs = new List<Vector2>();
        foreach (var dir in moveDirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, LayerMask.GetMask("Obstacle"));

            if (hit.collider == null)
            {
                validDirs.Add(dir);
            }
            
        }

        if (validDirs.Count > 0)
        {
            moveDir = validDirs[Random.Range(0, validDirs.Count)];
        }
        else
        {
            moveDir = Vector2.zero; // Không có hướng nào đi được
        }
        
        // Debug cho hướng di chuyển
        switch (moveDir) {
            case { x: 0, y: 1 }:
                Debug.Log($"Đổi hướng: Lên");
                break;
            case { x: 0, y: -1 }:
                Debug.Log($"Đổi hướng: Xuống");
                break;
            case { x: 1, y: 0 }: 
                Debug.Log($"Đổi hướng: Phải");  
                break;
            case { x: -1, y: 0 }:
                Debug.Log($"Đổi hướng: Trái");
                break;
        }
    }
}