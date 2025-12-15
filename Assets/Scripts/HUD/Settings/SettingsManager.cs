using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private List<SettingsWrapper> settings;
    
    private Dictionary<ESettingType, GameObject> _settingDict;
    private GameObject _currentSettings;

    private void Awake() {
        _settingDict = new Dictionary<ESettingType, GameObject>();
        foreach (SettingsWrapper settingWrapper in settings)
            _settingDict.TryAdd(settingWrapper.type, settingWrapper.settings);
    }

    public void SwitchControls(ESettingType _type) {
        if (!_settingDict.TryGetValue(_type, out var _value)) return;
        if (_value == _currentSettings) return;
        
        _currentSettings.SetActive(false);
        
        scrollView.verticalNormalizedPosition = 0;
        
        _currentSettings = _settingDict[_type];
        _currentSettings.SetActive(true);
    }
}
