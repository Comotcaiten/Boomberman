using System.Collections.Generic;
using UnityEngine;

public class OnealEnemy : Enemy
{
    private float directionChangeInterval = 2.0f;
    private float timer;

    // private float []speeds = { 0f };
    private float []speeds = { 1.0f, 2.0f, 3.0f };

    private bool isChasing = false;

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

        isChasing = false;
        foreach (var dir in moveDirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 5f, LayerMask.GetMask("Player"));

            if (hit.collider != null)
            {
                isChasing = true;
                moveDir = dir;
                Debug.DrawRay(transform.position, dir, Color.red, 0.5f);
                break;
            }
        }

        // Nếu không rượt đuổi thì mới đổi hướng sau mỗi khoảng thời gian
        if (!isChasing && timer <= 0f)
        {
            timer = directionChangeInterval;
            Change();
        }

        // Xử lý va chạm khi không rượt đuổi
        if (!isChasing)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.5f, obstacleLayer);
            if (hit.collider != null && hit.collider != collision2D)
            {
                Debug.DrawRay(transform.position, moveDir, Color.red, 0.5f);
                moveDir = -moveDir;
            }
        }
    }

    protected override void Change()
    {
        moveSpeed = speeds[Random.Range(0, speeds.Length)];
        if (isChasing) return;

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
            // fallback: đảo hướng hiện tại nếu đang có hướng, hoặc random
            if (moveDir != Vector2.zero)
                moveDir = -moveDir;
            else
                moveDir = moveDirs[Random.Range(0, moveDirs.Length)];
        }

    }
}