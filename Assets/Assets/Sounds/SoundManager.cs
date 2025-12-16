using System;
using System.Collections.Generic;
using UnityEngine;

public struct AudioByEnum
{
    public SoundName SoundName;
    public AudioClip audioClip;
}

public struct AudioSourceByType
{
    public SoundOrigin origin;
    public List<AudioSource> sources;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioByEnum> soundList;
    private Dictionary<SoundName, AudioClip> soundDict;
    [SerializeField] private static SoundManager instance;
    
    public static Action<SoundName, Vector3> OnSoundSpatializedPlayed;
    public static Action<SoundName> OnSoundPlayed;
    
    private AudioSource audioSource;

    [SerializeField] private List<AudioSourceByType> audioList;
    private Dictionary<SoundOrigin, List<AudioSource>> audioDict;

    private void Awake()
    {
        instance = this; // Ajouter la logique d'une instance
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        foreach (AudioByEnum sound in soundList)
        {
            if (soundDict.ContainsKey(sound.SoundName)) continue;
            soundDict.Add(sound.SoundName, sound.audioClip);
        }
        
        foreach (AudioSourceByType audio in audioList)
        {
            if (audioDict.ContainsKey(audio.origin)) continue;
            audioDict.Add(audio.origin, audio.sources);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OnSoundPlayed += PlaySoundGlobal;
        OnSoundSpatializedPlayed += PlaySoundSpatialized;
    }

    private void OnDisable()
    {
        OnSoundPlayed -= PlaySoundGlobal;
        OnSoundSpatializedPlayed -= PlaySoundSpatialized;
    }

    private void PlaySoundGlobal(SoundName sound)
    {
        Debug.Log("PlaySoundGlobal: " + sound);
        audioSource.PlayOneShot(soundDict[sound], 1f); // change volume by channel
    }
    private void PlaySoundSpatialized(SoundName sound ,Vector3 position)
    {
        Debug.Log("PlaySoundSpatialized: " + sound);
        audioSource.PlayOneShot(soundDict[sound], 1f); // change volume by channel
    }
}
