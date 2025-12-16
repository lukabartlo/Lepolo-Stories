using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : UICanvas {
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private List<SettingsWrapper> settings;
    
    private Dictionary<ESettingType, GameObject> _settingDict =  new ();
    private GameObject _currentSettings;

    private void Awake() {
        foreach (SettingsWrapper settingWrapper in settings)
            _settingDict.TryAdd(settingWrapper.type, settingWrapper.settings);
    }
    
    public override void OnCanvasOpen() {
        SwitchControls((int)ESettingType.Video);
    }

    public void SwitchControls(int _type) {
        if (!_settingDict.TryGetValue((ESettingType)_type, out var _value)) return;
        if (_value == _currentSettings) return;
        
        if(_currentSettings != null)
            _currentSettings.SetActive(false);
        
        scrollView.verticalNormalizedPosition = 0;
        
        _currentSettings = _settingDict[(ESettingType)_type];
        _currentSettings.SetActive(true);
    }
}
