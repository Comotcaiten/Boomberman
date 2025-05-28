using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource moveSource;

    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip getItemClip;

    public void PlayMove()
    {
        if (!moveSource.isPlaying)
        {
            moveSource.clip = moveClip;
            moveSource.loop = true;
            moveSource.Play();
        }
    }

    public void StopMove()
    {
        if (moveSource.isPlaying)
        {
            moveSource.Stop();
        }
    }

    public void PlayDeath()
    {
        effectSource.PlayOneShot(deathClip);
    }

    public void PlayGetItem()
    {
        effectSource.PlayOneShot(getItemClip);
    }
}
