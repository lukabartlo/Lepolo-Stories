using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellPanelFeedback : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI spellTitleText;
    [SerializeField] private TextMeshProUGUI spellDescriptionText;
    [SerializeField] private ContentSizeFitter contentSizeFitter;
    
    [SerializeField] private float timeToActivate;
    [SerializeField] private float fadeDuration;
    
    [SerializeField] private float yOffset;
    
    private CanvasGroup _canvasGroup;
    private Coroutine _activateCoroutine;

    private void Start() {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenFeedbackPanel(string _spellTitle, string _spellDescription, Vector3 _position) {
        if(_activateCoroutine != null)
            StopCoroutine(_activateCoroutine);
        
        spellTitleText.text = _spellTitle;
        spellDescriptionText.text = _spellDescription;
        transform.position = _position +  new Vector3(0, yOffset, 0);
        
        contentSizeFitter.enabled = false;
        
        _activateCoroutine = StartCoroutine(ActivationCoroutine());
    }
    
    public void CloseFeedbackPanel() {
        if(_activateCoroutine != null)
            StopCoroutine(_activateCoroutine);
        
        _canvasGroup.DOFade(0f, fadeDuration/10);
    }
    
    private IEnumerator ActivationCoroutine() {
        float _elapsedTime = 0f;
        while (_elapsedTime < timeToActivate) {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        contentSizeFitter.enabled = true;

        _canvasGroup.DOFade(1f, fadeDuration);
        _activateCoroutine =  null;
    }
}