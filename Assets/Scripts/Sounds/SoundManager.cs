using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [Header("Pooling")]
    [SerializeField] private int maxGlobalSources = 10;
    [SerializeField] private GameObject audioContainer;

    private Dictionary<SoundName, AudioClip> soundDict;
    private Dictionary<SoundOrigin, List<AudioSource>> audioDict;

    public static Action<SoundName> OnSoundPlayed;
    public static Action<SoundName, AudioSource> OnSoundSpatializedPlayed;

    #region Unity LifeCycle

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

    private void Start()
    {
        //PlaySoundGlobal(SoundName.Wind, true);
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

    #endregion

    #region Initialization

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
                audioDict.Add(audio.origin, new List<AudioSource>(audio.sources));
        }
    }

    #endregion

    #region Audio Factory

    /// <summary>
    /// Factory centralisée pour créer des AudioSource cohérentes
    /// </summary>
    private AudioSource CreateAudioSource(
        SoundOrigin origin,
        bool spatialized,
        float volume = 1f
    )
    {
        AudioSource source = audioContainer.AddComponent<AudioSource>();

        source.outputAudioMixerGroup = audioMixerGroup;
        source.playOnAwake = false;
        source.loop = false;
        source.volume = volume;
        source.pitch = 1f;

        if (spatialized)
        {
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 1f;
            source.maxDistance = 15f;
        }
        else
        {
            source.spatialBlend = 0f;
        }

        return source;
    }

    #endregion

    #region Play Sounds

    private void PlaySoundGlobal(SoundName sound)
    {
        if (!soundDict.TryGetValue(sound, out AudioClip clip))
        {
            Debug.LogWarning($"[SoundManager] Sound {sound} not found");
            return;
        }

        AudioSource source = GetAvailableAudioSource(SoundOrigin.Global);

        // Petit pitch aléatoire clean
        source.pitch = UnityEngine.Random.Range(0.90f, 1.1f);
        source.PlayOneShot(clip);
        Debug.Log($"Play sound : {sound}");
    }
    
    private void PlaySoundGlobal(SoundName sound, bool isLooping)
    {
        if (!soundDict.TryGetValue(sound, out AudioClip clip))
        {
            Debug.LogWarning($"[SoundManager] Sound {sound} not found");
            return;
        }

        AudioSource source = GetAvailableAudioSource(SoundOrigin.Global);

        // Petit pitch aléatoire clean
        source.loop = isLooping;
        source.pitch = UnityEngine.Random.Range(0.90f, 1.1f);
        source.PlayOneShot(clip);
        Debug.Log($"Play sound : {sound}");
    }

    private void PlaySoundSpatialized(SoundName sound, AudioSource source)
    {
        if (source == null)
            return;

        if (!soundDict.TryGetValue(sound, out AudioClip clip))
        {
            Debug.LogWarning($"[SoundManager] Sound {sound} not found");
            return;
        }

        source.pitch = UnityEngine.Random.Range(0.90f, 1.1f);
        source.PlayOneShot(clip);
        Debug.Log($"Play sound : {sound}");
    }

    #endregion

    #region Pooling

    private AudioSource GetAvailableAudioSource(SoundOrigin origin)
    {
        List<AudioSource> sources = audioDict[origin];

        // 1️⃣ Cherche une source libre
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
                return source;
        }

        // 2️⃣ Crée une nouvelle si possible
        if (origin == SoundOrigin.Global && sources.Count < maxGlobalSources)
        {
            AudioSource newSource = CreateAudioSource(origin, spatialized: false);
            sources.Add(newSource);
            return newSource;
        }

        // 3️⃣ Fallback (coupe le plus ancien)
        return sources[0];
    }

    #endregion
}
