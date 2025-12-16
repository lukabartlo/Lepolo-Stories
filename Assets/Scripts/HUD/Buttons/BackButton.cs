using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void OnBackButton() {
        UIManager.Instance.ClosePanel();
    }
}
