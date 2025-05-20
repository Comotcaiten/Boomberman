using System.Collections;
using UnityEngine;

public class SoundDestroy : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundDestroy;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(soundDestroy);
        yield return new WaitForSeconds(soundDestroy.length);
        Destroy(gameObject);
    }
    
}
