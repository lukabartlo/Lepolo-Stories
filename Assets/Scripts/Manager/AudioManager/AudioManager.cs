using System;
using System.Collections.Generic;
using Managers.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        [Header("Music Settings")]
        [SerializeField] private Transform _musicParent;

        [SerializeField] private GameObject _prefabMusicLink;
        [SerializeField] private List<AudioClipEntry> _musicEntries = new();

        [Space(30)]
        [Header("SFX Settings")]
        [SerializeField] private Transform _sfxParent;

        [SerializeField] private GameObject _prefabSfxLink;
        [SerializeField] private List<AudioClipEntry> _sfxEntries = new();

        private bool _isSfxPaused;
        private AudioSource _musicAudioSource;
        private AudioSource _sfxAudioSource;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitPrefabsLink();
            InitListeners();
            SceneManager.sceneLoaded += (_, _) => { InitListeners(); };
        }

        private void Update()
        {
            // Teste si l'on appuie sur S pour mettre en pause ou reprendre les effets sonores
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (_isSfxPaused)
                    UnpauseSfx();
                else
                    PauseSfx();
            }
        }


        private void InitPrefabsLink()
        {
            _musicAudioSource = Instantiate(_prefabMusicLink, _musicParent).GetComponent<AudioSource>();
            _sfxAudioSource = Instantiate(_prefabSfxLink, _sfxParent).GetComponent<AudioSource>();
        }

        /// <summary>
        ///     Adds the listeners to the event manager for audio related events
        /// </summary>
        private void InitListeners()
        {
            // Music
            EventManager.instance.onPlayMusic.AddListener(PlayMusic);
            EventManager.instance.onPauseMusic.AddListener(PauseMusic);
            EventManager.instance.onStopMusic.AddListener(StopMusic);

            // Sfx
            EventManager.instance.onPlaySfx.AddListener(PlaySfx);
            EventManager.instance.onPauseSfx.AddListener(PauseSfx);
            EventManager.instance.onStopSfx.AddListener(StopSfx);
        }

        private AudioClip FindClip(List<AudioClipEntry> entries, SoundsName name)
        {
            AudioClipEntry entry = entries.Find(e => e.key == name);
            if (entry != null && entry.clips.Count > 0) return entry.clips[Random.Range(0, entry.clips.Count)];
            return null;
        }

        #region Music

        private void PlayMusic(SoundsName name)
        {
            AudioClip clip = FindClip(_musicEntries, name);
            if (!clip)
            {
                Debug.LogWarning($"No music clip found for {name}");
                return;
            }

            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
        }

        private void PauseMusic()
        {
            if (_musicAudioSource.isPlaying) _musicAudioSource.Pause();
        }

        private void StopMusic()
        {
            if (!_musicAudioSource.isPlaying) return;
            _musicAudioSource.Stop();
        }

        #endregion

        #region SFX

        private void PlaySfx(SoundsName name)
        {
            AudioClip clip = FindClip(_sfxEntries, name);
            if (!clip)
            {
                Debug.LogWarning($"No SFX clip found for {name}");
                return;
            }

            // Utilise PlayOneShot pour jouer sans écraser le son précédent
            _sfxAudioSource.PlayOneShot(clip);
        }

        private void PauseSfx()
        {
            if (!_sfxAudioSource.isPlaying) return;
            _sfxAudioSource.Pause();
            _isSfxPaused = true;
        }

        private void UnpauseSfx()
        {
            _sfxAudioSource.UnPause();
            _isSfxPaused = false; // Marquer les SFX comme non-pause
        }

        private void StopSfx()
        {
            if (!_sfxAudioSource.isPlaying) return;
            _sfxAudioSource.Stop();
        }

        #endregion
    }
}


[Serializable]
public class AudioClipEntry
{
    public SoundsName key;
    public List<AudioClip> clips = new();
}