using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AudioByEnum
{
    public SoundName SoundName;
    public AudioClip audioClip;
}

[Serializable]
public struct AudioSourceByType
{
    public SoundOrigin origin;
    public List<AudioSource> sources;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sounds")]
    [SerializeField] private List<AudioByEnum> soundList;

    [Header("Audio Sources")]
    [SerializeField] private List<AudioSourceByType> audioList;

    private Dictionary<SoundName, AudioClip> soundDict;
    private Dictionary<SoundOrigin, List<AudioSource>> audioDict;

    public static Action<SoundName> OnSoundPlayed;
    public static Action<SoundName, AudioSource> OnSoundSpatializedPlayed;

    private void Awake()
    {
        // Singleton sécurisé
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        soundDict = new Dictionary<SoundName, AudioClip>();
        audioDict = new Dictionary<SoundOrigin, List<AudioSource>>();

        foreach (var sound in soundList)
        {
            if (!soundDict.ContainsKey(sound.SoundName))
                soundDict.Add(sound.SoundName, sound.audioClip);
        }

        foreach (var audio in audioList)
        {
            if (!audioDict.ContainsKey(audio.origin))
                audioDict.Add(audio.origin, audio.sources);
        }
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
        if (!soundDict.TryGetValue(sound, out AudioClip clip))
        {
            Debug.LogWarning($"Sound {sound} not found");
            return;
        }

        AudioSource source = GetAvailableAudioSource();
        source.PlayOneShot(clip);
    }

    private void PlaySoundSpatialized(SoundName sound, AudioSource source)
    {
        if (source == null)
            return;

        if (!soundDict.TryGetValue(sound, out AudioClip clip))
        {
            Debug.LogWarning($"Sound {sound} not found");
            return;
        }

        source.PlayOneShot(clip);
    }

    private AudioSource GetAvailableAudioSource() //       <================= A CHANGER
    {
        // Simple fallback : premier AudioSource global
        return audioDict[SoundOrigin.Global][0];
    }
}
