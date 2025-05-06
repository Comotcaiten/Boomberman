using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float horizontalInput;
    public float verticalInput;
    public GameObject bomPrefab;
    private Rigidbody2D playerRb;

    int bombRemaining;
    int bombAmount = 1;
    float bombFuseTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();

        bombRemaining = bombAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (bombRemaining > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBomb();
        }
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


    IEnumerator PlaceBomb() 
    {

        Vector2 position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        Instantiate(bomPrefab, position, bomPrefab.transform.rotation);

        bombRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        bombRemaining++;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"name: {collision.gameObject.name}");
    }
}
