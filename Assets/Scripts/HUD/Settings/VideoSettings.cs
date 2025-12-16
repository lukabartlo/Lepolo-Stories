using NUnit.Framework;
using TMPro;
using UnityEngine;

public class VideoSettings : MonoBehaviour {
    [SerializeField] private TMP_Dropdown fullscreenDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start() {
        Assert.IsNotNull(fullscreenDropdown, "fullscreen dropdown is null in Settings");
        Assert.IsNotNull(resolutionDropdown, "resolution Dropdown is null in Settings");
    }
    
    public void SetGraphicsSettings(string _label) {
        
    }

    public void SetResolutionSettings() {
        string[] res = resolutionDropdown.options[resolutionDropdown.value].text.Split('x');
        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[1]), Screen.fullScreen);
    }

    public void SetFullscreenSettings() {
        switch (fullscreenDropdown.options[fullscreenDropdown.value].text) {
            case "Fullscreen":
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case  "Fullscreen Windowed":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case "Windowed":
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
}
