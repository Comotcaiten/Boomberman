using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 2.0f;

    protected Vector2 moveDir;
    protected Rigidbody2D rb;
    protected CircleCollider2D collision2D;


    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        collision2D = GetComponent<CircleCollider2D>();
    }

    protected virtual void FixedUpdate() {

        Move();
        
    }

    protected virtual void Move() {
        rb.linearVelocity = moveDir * moveSpeed;
    }

    protected abstract void Change();

}