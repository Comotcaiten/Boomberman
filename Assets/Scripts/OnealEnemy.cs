using UnityEngine;

public class OnealEnemy : Enemy
{
    private float directionChangeInterval = 2.0f;
    private float timer;

    private Transform player;
    private float minSpeed = 1.5f;
    private float maxSpeed = 3.0f;

    private bool isChasing = false;

    private Vector2[] moveDirs = {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    private float detectionRange = 5.0f;
    private LayerMask obstacleMask;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        timer = directionChangeInterval;

        // Mask tường nếu bạn có layer riêng cho tường
        obstacleMask = LayerMask.GetMask("Obstacle");

        ChangeRandom();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isFainted) return;

        timer -= Time.fixedDeltaTime;

        if (timer <= 0f && !isChasing)
        {
            timer = directionChangeInterval;

            if (!isChasing)
            {
                Debug.Log("Oneal không thấy người chơi -> đi ngẫu nhiên");
                ChangeRandom();
            }   
        }
        if (CanSeePlayer())
        {
            Debug.Log("Oneal phát hiện người chơi -> đuổi theo");
            ChasePlayer();
        }
        
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > detectionRange)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleMask);
        
        return hit.collider == null; // Không bị chắn thì thấy được
    }

    void ChasePlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();

        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
            moveDir = new Vector2(Mathf.Sign(directionToPlayer.x), 0);
        else
            moveDir = new Vector2(0, Mathf.Sign(directionToPlayer.y));

        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void ChangeRandom()
    {
        int path = Random.Range(0, moveDirs.Length);
        moveDir = moveDirs[path];
        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    protected override void Change() {} // Không dùng Change gốc
}
