using System.Collections.Generic;
using UnityEngine;

public class BallomEnemy : Enemy
{
    private float directionChangeInterval = 1.0f;
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
        if (isDead) return;
        base.FixedUpdate();

        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            // Debug.Log($"Đổi hướng");

            timer = directionChangeInterval;
            Change();
        }
    }

    protected override void Change()
    {

        List<Vector2> validDirs = new List<Vector2>();
        foreach (var dir in moveDirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.5f, obstacleLayer);

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
    }
}