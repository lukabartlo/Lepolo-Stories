using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsAnimation : MonoBehaviour {
    [SerializeField] private Image buttonSpriteRenderer;
    
    [SerializeField] private Color baseSpriteColor;
    [SerializeField] private Color hoveredSpriteColor;
    [SerializeField] private Color pressedSpriteColor;
    
    [SerializeField] private float duration;
    
    public void OnPointerEnter() {
        buttonSpriteRenderer.DOColor(hoveredSpriteColor, duration);
    }

    public void OnPointerExit() {
        buttonSpriteRenderer.DOColor(baseSpriteColor, duration);
    }

    public void OnPointerDown() {
        buttonSpriteRenderer.DOColor(pressedSpriteColor, duration);
    }
}
