using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration;
    
    public PauseMenuHandler pauseMenuHandler;
    
    public void OpenCanvas() {
        pauseMenuHandler.OnOpenClosePanel(canvasGroup, 1, fadeDuration, true);
    }
}
