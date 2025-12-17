using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpTuto : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private int maxPage;
    
    [SerializeField] private float textDuration;
    [SerializeField] private CanvasGroup buttonGroup;
    
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

    public void ShowText() {
        popUpText.DOFade(1f, textDuration);
        buttonGroup.DOFade(1f, textDuration);
        buttonGroup.interactable = true;
        buttonGroup.blocksRaycasts = true;
    }
}
