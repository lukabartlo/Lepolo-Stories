using UnityEngine;
using System.Collections.Generic;

public enum SoundsTypes
{
    AMBIANT,
    SPELL,
    ACTIONS
};

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioList;
    [SerializeField] private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = instance;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySounds(int index)
    {
        audioSource.PlayOneShot(audioList[0]);
    }
}
