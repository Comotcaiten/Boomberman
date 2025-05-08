using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    private Vector2 moveInput = Vector2.zero;
    private Rigidbody2D playerRb;

    [SerializeField] private Animator animator;

    private bool isFainted = false;

    private Vector2 lastMovePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        MoveInput();
    }

    void MoveInput()
    {
        if (isFainted) return;
        
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        animator.SetFloat("horizontal", moveInput.x);
        animator.SetFloat("vertical", moveInput.y);
        animator.SetFloat("speed", (moveInput.x * moveInput.x + moveInput.y * moveInput.y));

        playerRb.linearVelocity = ((Vector2.up * moveInput.y) + (Vector2.right * moveInput.x)) * speed;

        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Ưu tiên hướng ngang
                lastMovePos.x = moveInput.x/Mathf.Abs(moveInput.x);
                lastMovePos.y = 0;
            }
            else
            {
                // hướng dọc
                lastMovePos.x = 0;
                lastMovePos.y = moveInput.y/Mathf.Abs(moveInput.y);
            }

            animator.SetFloat("LastPosX", lastMovePos.x);
            animator.SetFloat("LastPosY", lastMovePos.y);
        }

    }

    public void SetIsFainted(bool value)
    {
        if (isFainted == true) return;
        if (value == true) {
            animator.SetBool("IsDead", true);
        }
        isFainted = value;
    }

    public bool GetIsFainted() { return isFainted; }

}
