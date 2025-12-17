using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    [SerializeField] private SpellData spellData;
    [SerializeField] private GameManager gm;
    [SerializeField] private Button button;
    private Image buttonBackground;
    
    [SerializeField] private SpellPanelFeedback feedBackPanel;
    
    void Start()
    {
        gm = GameManager.Instance;
        GetComponent<Button>().onClick.AddListener(OnClick);
        buttonBackground = GetComponent<Image>();
    }

    private void Update()
    {
        if (gm.currentMana <= spellData.spellCost)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void FeedBack(bool _doFeedback)
    {
        if (!_doFeedback)
        {
            feedBackPanel.CloseFeedbackPanel();
            return;
        }
        
        feedBackPanel.OpenFeedbackPanel(spellData.spellName, spellData.spellDescription, transform.position);
    }

    public void OnClick()
    {
        SpellManager.Instance.ActivateSpell(spellData);
    }
}