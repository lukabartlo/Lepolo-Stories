
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

    }
    
    public void ChangeMusicVolume(string _audioName) {
        Slider slider = null;
        
        switch (_audioName) {
            case "Master":
                slider = masterVol;
                break;
            case "Music":
                slider = musicVol;
                break;
            case "Sfx":
                slider = sfxVol;
                break;
        }
        
        if(slider != null)
            mainAudioMixer.SetFloat(_audioName, Mathf.Log10(slider.value) * 20);
    }
    
}
