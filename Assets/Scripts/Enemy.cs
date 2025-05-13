using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] protected float speed = 2.0f;
    protected float moveSpeed;

    protected Vector2 moveDir;
    protected Rigidbody2D enenmyRb;
    protected CircleCollider2D collision2D;
    public bool isFainted {get; private set;} = false;

    [SerializeField] protected Animator animator;
    protected virtual void Start() {
        enenmyRb = GetComponent<Rigidbody2D>();
        collision2D = GetComponent<CircleCollider2D>();
        moveSpeed = speed;
    }

    protected virtual void FixedUpdate() {
        if (isFainted) return;
        Move();
    }

    protected virtual void Move() {
        enenmyRb.linearVelocity = moveDir * moveSpeed;
    }

    protected abstract void Change();

    IEnumerator DeathAfter() {

        // Chạy animation chết
        PlayDeathAnimation();

        yield return new WaitForSeconds(1.0f);

        // Destroy(gameObject);
        gameObject.SetActive(false);

        // Gọi hàm trong GameManager để cập nhật số lượng kẻ địch còn lại
        GameManager.Instance.UpdateEnemyCount();
        
    }

    public void SetIsFainted(bool value) {
        isFainted = value;

        if (isFainted) {
            
            Debug.Log($"Enemy {gameObject.name} đã bị hạ gục");

            StartCoroutine(DeathAfter());

            FreezeMovement();
        }
    }

    protected void PlayDeathAnimation() {

        if (animator == null) {
            Debug.Log("Animator is not assigned.");
            return;
        }
        animator.SetBool("IsDeath", true);
    }

    public void FreezeMovement() {
        // Đặt vị trí của enemy về vị trí gần nhất với ô lưới
        // Để tránh việc enemy bị kẹt vào tường
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        enenmyRb.constraints = RigidbodyConstraints2D.FreezeAll; // Ngăn không cho di chuyển
        moveSpeed = 0f; // Đặt tốc độ về 0
        enenmyRb.linearVelocity = Vector2.zero; // Đặt vận tốc về 0
    }

    public void UnFreezeMovement() {
        enenmyRb.constraints = RigidbodyConstraints2D.None; // Cho phép di chuyển
        moveSpeed = 2f; // Đặt tốc độ về 2
    }

}