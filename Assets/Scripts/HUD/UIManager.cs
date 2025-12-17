using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    
    [SerializeField] private float closingDuration;
    [SerializeField] private CanvasGroup pausePanel;
    
    private List<CanvasGroup> _openCanvas = new List<CanvasGroup>();
    private Coroutine _coroutine;

    private void Awake() {
        Instance = this;
    }
    
    public void HandleEscape() {
        if (_openCanvas.Count == 0) {
            if(pausePanel)
                OnOpenClosePanel(pausePanel, 1, closingDuration,true);
            return;
        }
        
        ClosePanel();
    }
    
    public void ClosePanel() {
        if(_openCanvas.Count > 0)
            OnOpenClosePanel(_openCanvas[^1], 0, closingDuration,false);
    }
    
    public void OnOpenClosePanel(CanvasGroup _canvasGroup, float _endValue, float _duration, bool _activate) {
        if (_coroutine != null) return;
        
        _canvasGroup.TryGetComponent(out UICanvas canvas);
        canvas?.OnCanvasOpen();

        _coroutine = StartCoroutine(FadeCanvas(_canvasGroup, _endValue, _duration, _activate));

        if (_activate) {
            _openCanvas.Add(_canvasGroup);
        } else {
            _openCanvas.Remove(_canvasGroup);
        }
    }
    
    public void OpenScene(string _levelName) {
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