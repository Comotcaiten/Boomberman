using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float horizontalInput;
    public float verticalInput;

    public GameObject bomPrefab;

    private Rigidbody2D playerRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MoveInput();
        PlaceBomb();
    }


    void MoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        playerRb.linearVelocity =  ((Vector2.up * verticalInput) + (Vector2.right * horizontalInput)) * speed;
    }


    void PlaceBomb() 
    {
        if (bomPrefab == null) return;
        if (Input.GetKey(KeyCode.Space))
        {
            Vector2 placePos = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Instantiate(bomPrefab, placePos, bomPrefab.transform.rotation);
        }
    }
}
