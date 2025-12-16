using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTestManager : MonoBehaviour {
    public static MainMenuTestManager Instance;
    
    [SerializeField] private MainMenuController mainMenuController;
    
    [SerializeField] private float closingDuration;
    
    private List<CanvasGroup> _openCanvas = new List<CanvasGroup>();
    private Coroutine _coroutine;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        mainMenuController.acquireCancelInput += ClosePanel;
    }

    public void ClosePanel() {
        if(_openCanvas.Count > 0)
            OnOpenClosePanel(_openCanvas[^1], 0, closingDuration,false);
    }
    
    public void OnOpenClosePanel(CanvasGroup _canvasGroup, float _endValue, float _duration, bool _activate) {
        if (_coroutine != null) return;
        _coroutine = StartCoroutine(FadeCanvas(_canvasGroup, _endValue, _duration, _activate));

        if (_activate) {
            _openCanvas.Add(_canvasGroup);
        } else {
            _openCanvas.Remove(_canvasGroup);
        }
    }

    public void StartGame(string _levelName) {
        SceneManager.LoadScene(_levelName);
    }
    
    public void CloseGame() {
        Application.Quit();
    }
    
    private IEnumerator FadeCanvas(CanvasGroup _canvasGroup, float _endValue, float _duration, bool _activate) {
        float _elapsedTime = 0f;

        while (_elapsedTime < _duration) {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _endValue, (_elapsedTime / _duration));
            _elapsedTime +=  Time.deltaTime;
            yield return null;
        }
        
        _canvasGroup.blocksRaycasts = _activate;
        _canvasGroup.interactable = _activate;
        
        _canvasGroup.alpha = _endValue;
        
        _coroutine = null;
    }
}