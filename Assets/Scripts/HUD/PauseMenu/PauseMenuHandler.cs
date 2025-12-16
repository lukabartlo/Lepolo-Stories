using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour {
    private List<CanvasGroup> _openCanvas = new List<CanvasGroup>();
    private Coroutine _coroutine;
    [SerializeField] private float closingDuration;

    private bool _isOpen = true;
    
    private void Start() {
        // mainMenuController.acquireCancelInput += HandleEscape;
    }
    
    public void OpenScene(string _levelName) {
        SceneManager.LoadScene(_levelName);
    }

    private void HandleEscape() {
        if (!_isOpen)
        {
            OnOpenClosePanel(GetComponent<CanvasGroup>(), 1f, closingDuration, true);
            _isOpen = true;
        }
        else {
            ClosePanel();
        }
    }
    
    public void ClosePanel() {
        if(_openCanvas.Count > 0)
            OnOpenClosePanel(_openCanvas[^1], 0, closingDuration,false);
        
        if(_openCanvas.Count == 0) _isOpen = false;
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
