using UnityEngine;

public class MainMenuButton : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public float fadeDuration;

    public void OpenCanvas() {
        MainMenuTestManager.Instance.OnOpenClosePanel(canvasGroup, 1, fadeDuration, true);
    }
}
