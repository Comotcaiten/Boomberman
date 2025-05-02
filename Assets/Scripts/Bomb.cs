using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    
    public float timeDestory;
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
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}   
