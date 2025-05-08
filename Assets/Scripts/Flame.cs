using System.Collections;
using UnityEngine;

public class Flame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyAfter() {
        yield return new WaitForSeconds(0.75f);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("GameOver");

            collision.gameObject.GetComponent<PlayerController>().SetIsFainted(true);
        }
        if (collision.gameObject.CompareTag("Enemy")) {
            Debug.Log("Kill Enemy");

            collision.gameObject.GetComponent<Enemy>();
        }
    }
}
