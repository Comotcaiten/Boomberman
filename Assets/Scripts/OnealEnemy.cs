using System.Collections.Generic;
using UnityEngine;

public class OnealEnemy : Enemy
{
    private float directionChangeInterval = 5.0f;
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

        foreach (var dir in moveDirs)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 5f, LayerMask.GetMask("Player"));

            if (hit.collider != null)
            {
                isChasing = true;
                moveDir = dir;
                Debug.DrawRay(transform.position, dir, Color.red, 5f);
                break;
            }

            isChasing = false;
        }

        if (timer <= 0f)
        {
            timer = directionChangeInterval;
            Change();
        }
    }

    protected override void Change()
    {
        moveSpeed = speeds[Random.Range(0, speeds.Length)];
        if (!isChasing)
        {
            moveDir = moveDirs[Random.Range(0, moveDirs.Length)];
        }
    }
}