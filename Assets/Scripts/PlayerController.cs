using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float moveSpeed;

    private Vector2 moveInput = Vector2.zero;
    private Rigidbody2D playerRb;

    [SerializeField] private Animator animator;

    public bool isFainted { get; private set; } = false;

    private Vector2 lastMovePos;

    private bool canControl = true;

    [SerializeField] private AudioClip playerAudioDeath;
    [SerializeField] private AudioClip playerAudioGetItem;
    [SerializeField] private AudioClip playerAudioMove;

    [SerializeField] private AudioSource audioSourceEffect;
    [SerializeField] private AudioSource audioSourceMove;
    private bool isMoving = false;

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
        if (isFainted || !canControl) return;

        // moveInput.x = Input.GetAxis("Horizontal");
        // moveInput.y = Input.GetAxis("Vertical");
        // if (moveInput != Vector2.zero)
        // {
        //     if (!isMoving)
        //     {
        //         // audioSourceMove.PlayOneShot(playerAudioMove);
        //         audioSourceMove.Play();
        //         isMoving = true;
        //     }
        // }
        // else
        // {
        //     audioSourceMove.Stop();
        //     isMoving = false;
        // }

        animator.SetFloat("horizontal", moveInput.x);
        animator.SetFloat("vertical", moveInput.y);
        animator.SetFloat("speed", (moveInput.x * moveInput.x + moveInput.y * moveInput.y));

        // moveInput.Normalize(); // Đưa về vector đơn vị
        playerRb.linearVelocity = ((Vector2.up * moveInput.y) + (Vector2.right * moveInput.x)) * moveSpeed; 

        // Vector2 move = moveInput * moveSpeed * Time.deltaTime;
        // transform.Translate(move);

        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Ưu tiên hướng ngang
                lastMovePos.x = moveInput.x / Mathf.Abs(moveInput.x);
                lastMovePos.y = 0;
            }
            else
            {
                // hướng dọc
                lastMovePos.x = 0;
                lastMovePos.y = moveInput.y / Mathf.Abs(moveInput.y);
            }

            animator.SetFloat("LastPosX", lastMovePos.x);
            animator.SetFloat("LastPosY", lastMovePos.y);
        }

    }

    public void SetIsFainted(bool value)
    {
        if (isFainted == true) return;
        if (value == true)
        {
            animator.SetBool("IsDead", true);

            GameManager.Instance.GameOver();

            FreezeMovement();
            audioSourceEffect.PlayOneShot(playerAudioDeath);
            canControl = false;
        }
        isFainted = value;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SetIsFainted(true);
        }
    }

    public void TakeItem(Item item)
    {
        if (item == null) return;
        item.gameObject.SetActive(false);
        audioSourceEffect.PlayOneShot(playerAudioGetItem);
        // Thực hiện hiệu ứng của item
        StartCoroutine(item.Effect());
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
        playerRb.constraints = RigidbodyConstraints2D.None; // Bỏ ràng buộc
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Giữ nguyên độ xoay
        moveSpeed = speed; // Đặt tốc độ về 5
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

}
