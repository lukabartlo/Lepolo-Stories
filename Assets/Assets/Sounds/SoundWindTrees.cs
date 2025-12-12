using UnityEngine;

public enum SoundType // only one sound per type
{
    WIND
}

[RequireComponent(typeof(AudioSource))]

public class SoundWindTrees : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private static SoundWindTrees instance;
    private AudioSource audioSource;

    private int timerWindSound = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySound(SoundType.WIND);
    }

    private void Update()
    {
        timerWindSound++;
        if (timerWindSound > 12500)
        {
            PlaySound(SoundType.WIND);
            timerWindSound = 0;
        }
    }

    public static void PlaySound(SoundType sound, float volume = 0.5f)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
