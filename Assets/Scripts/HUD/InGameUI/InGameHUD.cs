using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour {
    public static InGameHUD Instance;

    [SerializeField] private Image madnessBar;
    [SerializeField] private Image manaBar;
    
    [SerializeField] private TextMeshProUGUI adeptCounterText;
    
    [SerializeField] private CanvasGroup tuto;
    

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        UIManager.Instance.OnOpenClosePanel(tuto, 1, 0.3f, true);
    }

    public void SetMadness(List<float> _allMadness, float _maxMadness) {
        float _currentMadness = 0;
        foreach (float madness in _allMadness) 
            _currentMadness += madness;
        _currentMadness /= _allMadness.Count;
        
        madnessBar.fillAmount = _currentMadness / _maxMadness;
    }

    public void SetMana(float _currentMana, float _maxMana) {
        manaBar.fillAmount = _currentMana / _maxMana;
    }

    public void SetAdeptCounter(int _currentAdeptCounter, int _maxAdeptCounter) {
        string _staticString = "0";
        if (_currentAdeptCounter < 10)
            _staticString += "0";
        
        adeptCounterText.text = _staticString + _currentAdeptCounter;
    }
}
