using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CreditsCanvas : MonoBehaviour {
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float duration;
    [SerializeField] private float waitDurationBeforeStartingCredits;
    
    public AnimationCurve animationCurve;
    private Coroutine _scrollCoroutine;
    
    private void OnEnable() {
        if(_scrollCoroutine != null) StopCoroutine(_scrollCoroutine);
        
        scrollRect.verticalNormalizedPosition = 1;
        _scrollCoroutine = StartCoroutine(PlayCreditsAnimation());
    }

    private void OnAnimFinished() {
        scrollRect.gameObject.SetActive(false);
    }

    private IEnumerator PlayCreditsAnimation() {
        float _elapsedTime = 0f;
        
        while (_elapsedTime < duration) {
            
            float _valueOnCurve = animationCurve.Evaluate(_elapsedTime);
            scrollRect.verticalNormalizedPosition = _valueOnCurve;
            
            // Debug.Log(scrollRect.verticalNormalizedPosition = animationCurve.Evaluate(_elapsedTime));
            _elapsedTime +=  Time.deltaTime;
            yield return null;
        }
        OnAnimFinished();
    }
}
