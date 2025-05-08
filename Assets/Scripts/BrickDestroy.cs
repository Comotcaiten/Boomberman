using System.Collections;
using UnityEngine;
public class BrickDestroy : MonoBehaviour
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
}
