using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
    [SerializeField] private Slider masterVol;
    [SerializeField] private Slider musicVol;
    [SerializeField] private Slider sfxVol;
    [SerializeField] private AudioMixer mainAudioMixer;
    
    private void Start() {
        Assert.IsNotNull(masterVol, "master volume slider is null in Settings");
        Assert.IsNotNull(musicVol, "music volume slider is null in Settings");
        Assert.IsNotNull(sfxVol, "sfx volume slider is null in Settings");
        Assert.IsNotNull(mainAudioMixer, "main audio mixer is null in Settings");
    }
    
    public void ChangeMusicVolume(string _audioName) {
        mainAudioMixer.SetFloat(_audioName, Mathf.Log10(musicVol.value) * 20);
    }
    
}
