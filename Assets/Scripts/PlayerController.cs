using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float horizontalInput;
    public float verticalInput;
    private Rigidbody2D playerRb;

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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        playerRb.linearVelocity =  ((Vector2.up * verticalInput) + (Vector2.right * horizontalInput)) * speed;
    }
}
