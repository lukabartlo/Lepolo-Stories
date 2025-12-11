using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinksPanel : MonoBehaviour {
    [SerializeField] private GameObject linkButtonPrefab;
    
    [SerializeField] private Transform content;
    [SerializeField] private CanvasGroup contentCanvasGroup;
    
    [SerializeField] private float fadeDuration;
    [SerializeField] private float heightFactor;
    
    [SerializeField] private List<UrlTypeWrapper> urlSprites;
    private Dictionary<UrlType, Sprite> _urlSpriteDico;
    
    private Coroutine _coroutine;
    
    private GameObject _connectedGameObject; 

    private void Start() {
        _urlSpriteDico = new Dictionary<UrlType, Sprite>();
        foreach (var urlSprite in urlSprites) {
            _urlSpriteDico.TryAdd(urlSprite.urlType, urlSprite.urlSprite);
        }
    }

    public void OnOpenPanel(List<LinkWrapper> _links, GameObject _sender) {
        if (_connectedGameObject == _sender) return;
        _connectedGameObject = _sender;
        
        foreach (LinkWrapper link in _links) {
            LinkButton _button = Instantiate(linkButtonPrefab, content).GetComponent<LinkButton>();
            _button.SetupVariables(link.url, _urlSpriteDico[link.urlType]);
        }
        
        content.position = _sender.transform.position + Vector3.up * heightFactor;
        
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FadeCanvas(contentCanvasGroup, 1f, fadeDuration, true));
    }

    public void OnClosePanel() {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FadeCanvas(contentCanvasGroup, 0f, fadeDuration, false));
        
        for (int i = content.childCount - 1; i >= 0; i--) {
            Destroy(content.GetChild(i).gameObject);
        }
        
        _connectedGameObject = null;
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
        
        _coroutine = null;
    }
}
