using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float moveSpeed;

    private Vector2 moveInput = Vector2.zero;
    private Rigidbody2D playerRb;

    [SerializeField] private Animator animator;

    public bool isDead { get; private set; } = false;

    private Vector2 lastMovePos;

    private bool canControl = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        moveSpeed = speed;

    }

    void FixedUpdate()
    {
        MoveInput();
    }

    void MoveInput()
    {
        if (isDead || !canControl) return;

        animator.SetFloat("horizontal", moveInput.x);
        animator.SetFloat("vertical", moveInput.y);
        animator.SetFloat("speed", (moveInput.x * moveInput.x + moveInput.y * moveInput.y));

        // playerRb.MovePosition(playerRb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        playerRb.linearVelocity = moveInput * moveSpeed;


        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Ưu tiên hướng ngang
                // bỏ vì Gây lỗi chia 0 nếu moveInput.x == 0 (hiếm, nhưng có thể xảy ra nếu input dao động nhỏ).
                // lastMovePos.x = moveInput.x / Mathf.Abs(moveInput.x);
                // Mathf.Sign()
                lastMovePos.x = Mathf.Sign(moveInput.x);
                lastMovePos.y = 0;
            }
            else
            {
                // hướng dọc
                lastMovePos.x = 0;
                lastMovePos.y = Mathf.Sign(moveInput.y);
            }

            animator.SetFloat("LastPosX", lastMovePos.x);
            animator.SetFloat("LastPosY", lastMovePos.y);
        }

    }

    public void SetIsDead(bool value)
    {
        if (isDead == true) return;
        if (value == true)
        {
            animator.SetBool("IsDead", true);

            GameManager.Instance.GameOver();

            FreezeMovement();
            canControl = false;
        }
        isDead = value;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SetIsDead(true);

            Physics2D.IgnoreCollision(collision.collider, GetComponent<CircleCollider2D>());
        }
    }

    public void TakeItem(Item item)
    {
        if (item == null) return;
        // item.gameObject.SetActive(false);
        // Thực hiện hiệu ứng của item
        StartCoroutine(item.Effect());

        SoundManager.PlaySound(SoundType.GETITEM);
    }

    public void FreezeMovement()
    {
        // Đặt vị trí của player về vị trí gần nhất với ô lưới
        // Để tránh việc player bị kẹt vào tường
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll; // Ngăn không cho di chuyển
        moveSpeed = 0f; // Đặt tốc độ về 0
        playerRb.linearVelocity = Vector2.zero; // Đặt vận tốc về 0
    }

    public void UnfreezeMovement()
    {
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Giữ nguyên độ xoay
        moveSpeed = speed; // Đặt tốc độ về 5
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }
    
    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
        moveSpeed = speed;
    }

}
