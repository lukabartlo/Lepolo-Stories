using UnityEngine;

public class OpenCanvasButton : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration;
    
    public void OpenCanvas() {
        UIManager.Instance.OnOpenClosePanel(canvasGroup, 1, duration, true);
    }
}
