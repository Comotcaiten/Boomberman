using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 2.0f;

    protected Vector2 moveDir;
    protected Rigidbody2D rb;
    protected CircleCollider2D collision2D;
    protected bool isFainted = false;

    [SerializeField] protected Animator animator;
    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        collision2D = GetComponent<CircleCollider2D>();
    }

    protected virtual void Update() {
        if (isFainted) {
            return;
        }
    }

    protected virtual void FixedUpdate() {
        if (isFainted) {
            return;
        }
        Move();
    }

    protected virtual void Move() {
        rb.linearVelocity = moveDir * moveSpeed;
    }

    protected abstract void Change();

    IEnumerator DeathAfter() {

        // Chạy animation chết
        PlayDeathAnimation();

        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }

    public void SetIsFainted(bool value) {
        isFainted = value;

        if (isFainted) {
            moveSpeed = 0f;
            moveDir = Vector2.zero;

            StartCoroutine(DeathAfter());
        }
    }

    public bool GetIsFainted() {return isFainted;}

    protected void PlayDeathAnimation() {

        if (animator == null) {
            Debug.Log("Animator is not assigned.");
            return;
        }
        animator.SetBool("IsDeath", true);
    }

}