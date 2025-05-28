using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    MOVE,
    DEATH,
    GETITEM,
    PLACEBOMB,
    EXPLOSION,
    MUSIC,
}

[RequireComponent(typeof(AudioSource))] 
public class SoundManager : MonoBehaviour
{
    [SerializeField] private ListSoundClips listSoundClips;
    private static SoundManager Instance;
    private AudioSource audioSource;

    private AudioSource musicSource;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1.0f)
    {
        // Instance.audioSource.PlayOneShot(Instance.soundClips[(int)sound], volume);
        Instance.audioSource.PlayOneShot(Instance.listSoundClips.GetAudioClip(sound), volume);
    }
}


[Serializable]
public struct SoundClips
{
    public SoundType soundType;
    public AudioClip audioClip;
}

[Serializable]
public class ListSoundClips
{
    public List<SoundClips> soundClips;

    public ListSoundClips()
    {
        soundClips = new List<SoundClips>();
    }

    public void Add(SoundType soundType, AudioClip audioClip)
    {
        soundClips.Add(new SoundClips { soundType = soundType, audioClip = audioClip });
    }

    public AudioClip GetAudioClip(SoundType soundType)
    {
        foreach (var clip in soundClips)
        {
            if (clip.soundType == soundType)
            {
                return clip.audioClip;
            }
        }
        return null;
    }
}