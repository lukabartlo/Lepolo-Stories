using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI buttontext;
    [SerializeField] private float textFadeDuration;
    
    [SerializeField] private List<LinkWrapper> links = new List<LinkWrapper>();
    
    private Coroutine _coroutine;

    private void Start() {
        buttontext.alpha = 0;
    }

    public void OnHover() {
        StartFade(true);
    }
    
    public void OnExit() {
        StartFade(false);
    }

    public void OnClick() {
        transform.parent.GetComponent<LinksPanel>().OnOpenPanel(links, gameObject);
    }

    private void StartFade(bool _fadeIn) {
        if(_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FadeText(_fadeIn? 1 : 0));
    }

    private IEnumerator FadeText(float _end) {
        float _elapsedTime = 0f;

        while (_elapsedTime < textFadeDuration) {
            buttontext.alpha = Mathf.Lerp(textFadeDuration, _end, (_elapsedTime / textFadeDuration));
            _elapsedTime +=  Time.deltaTime;
            yield return null;
        }
    }
}