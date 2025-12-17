using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTuto : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private int maxPage;
    
    [SerializeField] private Button closeButton;
    
    private void Start() {
        closeButton.interactable = popUpText.pageToDisplay == maxPage;
        popUpText.pageToDisplay = 1;
    }

    public void PassPage(int _increment) {
        popUpText.pageToDisplay = Mathf.Clamp(popUpText.pageToDisplay + _increment, 1, maxPage);
        closeButton.interactable = popUpText.pageToDisplay == maxPage;
    }
}
