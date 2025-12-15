using EasyTextEffects;
using UnityEngine;

public class MainMenuButton : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public float fadeDuration;
    
    [SerializeField] private TextEffect textEffect;

    public void OpenCanvas() {
        MainMenuTestManager.Instance.OnOpenClosePanel(canvasGroup, 1, fadeDuration, true);
    }

    public void UseEffect(string _effectName) {
        textEffect.StartManualEffect(_effectName);
    }
    
    public void StopEffects() {
        textEffect.StopAllEffects();
    }
}
