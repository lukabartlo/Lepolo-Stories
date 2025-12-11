//using UnityEngine;

//public enum SoundType // only one sound per type
//{
//    AMBIANT,
//    SPELL,
//    BUTTON,
//    ACTION,
//    ATTACK
//}

//[RequireComponent(typeof(AudioSource))]

//public class TESTSoundManager : MonoBehaviour
//{
//    [SerializeField] private AudioClip[] soundList;
//    [SerializeField] private static SoundManager instance;
//    private AudioSource audioSource;

//    private void Awake()
//    {
//        instance = this;
//    }

//    private void Start()
//    {
//        audioSource = GetComponent<AudioSource>();
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.F)) {
//            PlaySound(SoundType.BUTTON);
//        }
//        if (Input.GetKeyDown(KeyCode.G)) {
//            PlaySound(SoundType.SPELL);
//        }
//        if (Input.GetKeyDown(KeyCode.H)) {
//            PlaySound(SoundType.ATTACK);
//        }
//    }

//    public static void PlaySound(SoundType sound, float volume = 1f)
//    {
//        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
//    }
//}
