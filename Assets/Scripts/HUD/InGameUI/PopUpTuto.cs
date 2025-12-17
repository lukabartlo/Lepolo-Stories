using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpTuto : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private int maxPage;
    
    [SerializeField] private Button closeButton;
    [SerializeField] private EventTrigger eventTrigger;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    private void Start() {
        popUpText.pageToDisplay = 1;
        closeButton.interactable = eventTrigger.enabled = popUpText.pageToDisplay == maxPage;
        buttonText.color = eventTrigger.enabled? Color.black : Color.grey;
    }

    public void PassPage(int _increment) {
        popUpText.pageToDisplay = Mathf.Clamp(popUpText.pageToDisplay + _increment, 1, maxPage);
        closeButton.interactable = eventTrigger.enabled = popUpText.pageToDisplay == maxPage;
        buttonText.color = eventTrigger.enabled? Color.black : Color.grey;
    }
}
