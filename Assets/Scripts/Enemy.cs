using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] protected float speed = 2.0f;
    protected float moveSpeed;

    protected Vector2 moveDir;
    protected Rigidbody2D enenmyRb;
    protected CircleCollider2D collision2D;
    public bool isDead { get; private set; } = false;

    [SerializeField] protected Animator animator;

    private Vector2 inputMove = Vector2.zero;

    public int score;
    private Score ScorePoint;
    protected LayerMask obstacleLayer;

    [Range(1, 100)] public int bombRemainToKill = 1; // Số lượng bom cần để tiêu diệt enemy này
    private int bombAmount = 0; // Số lượng bom đã sử dụng để tiêu diệt enemy này

    public SpriteRenderer spriteRenderer;

    private bool isInvulnerable = false;

    protected virtual void Start()
    {
        ScorePoint = FindAnyObjectByType<Score>();
        enenmyRb = GetComponent<Rigidbody2D>();
        collision2D = GetComponent<CircleCollider2D>();
        moveSpeed = speed;

        obstacleLayer = LayerMask.GetMask("Obstacle");

        bombAmount = bombRemainToKill;
    }


    protected virtual void FixedUpdate()
    {
        if (isDead) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.5f, obstacleLayer);
        if (hit.collider != null && hit.collider != collision2D)
        {
            // Debug.Log($"Hit: {hit.collider.name}");
            // Debug.DrawRay(transform.position, moveDir, Color.red, 1f);

            moveDir = Vector2.zero;
        }
        MoveAnimator();
        Move();
    }

    protected virtual void Move()
    {
        enenmyRb.linearVelocity = moveDir * moveSpeed;
    }

    protected virtual void MoveAnimator()
    {
        if (animator == null)
        {
            Debug.Log("Animator is not assigned.");
            return;
        }

        if (inputMove.x != moveDir.x || inputMove.y != moveDir.y)
        {
            inputMove = new Vector2(moveDir.x, moveDir.y);
            if (inputMove.x != 0) { inputMove.x = (Mathf.Abs(inputMove.x) / inputMove.x); }
            if (inputMove.y != 0) { inputMove.y = (Mathf.Abs(inputMove.y) / inputMove.y); }
        }

        animator.SetFloat("horizontal", moveDir.x);
        animator.SetFloat("vertical", moveDir.y);
        animator.SetFloat("speed", (moveDir.x * moveDir.x + moveDir.y * moveDir.y));

        animator.SetFloat("lastHorizontal", inputMove.x);
        animator.SetFloat("lastVertical", inputMove.y);
    }

    protected abstract void Change();

    IEnumerator DeathAfter()
    {

        // Chạy animation chết
        PlayDeathAnimation();

        yield return new WaitForSeconds(1.0f);

        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void SetIsDead(bool value)
    {
        if (isDead == true) return;
        isDead = value;

        if (isDead)
        {
            Debug.Log($"Enemy {gameObject.name} đã bị hạ gục");
            StartCoroutine(DeathAfter());
            ScorePoint.AddScore(score);
            FreezeMovement();
        }
    }

    protected void PlayDeathAnimation()
    {

        if (animator == null)
        {
            Debug.Log("Animator is not assigned.");
            return;
        }

        animator.SetBool("IsDeath", true);
    }

    public void FreezeMovement()
    {
        // Đặt vị trí của enemy về vị trí gần nhất với ô lưới
        // Để tránh việc enemy bị kẹt vào tường
        // transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        enenmyRb.constraints = RigidbodyConstraints2D.FreezeAll; // Ngăn không cho di chuyển
        moveSpeed = 0f; // Đặt tốc độ về 0
        enenmyRb.linearVelocity = Vector2.zero; // Đặt vận tốc về 0
    }

    public void UnFreezeMovement()
    {
        enenmyRb.constraints = RigidbodyConstraints2D.None; // Cho phép di chuyển
        moveSpeed = 2f; // Đặt tốc độ về 2
    }

    public void TakeDamage()
    {
        if (isDead || isInvulnerable) return; // Không bị thương khi đang miễn sát thương
        bombAmount--;
        if (bombAmount <= 0)
        {
            SetIsDead(true);
            InvulnerabilityDelay(0.5f);
            StopForSecondsByFlame(1f);
            return;
        }

        if (spriteRenderer != null)
        {
            // Đổi màu nhạt dần theo tổng số bomb đã nhận / tổng bomb cần nhận
            float colorValue = (float)bombAmount / bombRemainToKill;
            // Giữ nguyên màu sắc ban đầu, chỉ thay đổi alpha
            spriteRenderer.color = new Color(1f, 1f, 1f, colorValue);
        }

        InvulnerabilityDelay(0.5f);
        StopForSecondsByFlame(1f);
    }

    IEnumerator StopForSecondsByFlame(float seconds)
    {
        FreezeMovement();
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        // Đặt vận tốc về 0
        enenmyRb.linearVelocity = Vector2.zero; // Đặt vận tốc về 0
        yield return new WaitForSeconds(seconds);
        UnFreezeMovement();
    }

    private IEnumerator InvulnerabilityDelay(float time)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(time);
        isInvulnerable = false;
    }

}