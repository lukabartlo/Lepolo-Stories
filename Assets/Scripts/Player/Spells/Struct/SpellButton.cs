using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    [SerializeField] private SpellData spellData;
    [SerializeField] private GameManager gm; // for the moment
    [SerializeField] private Button button;
    private Image buttonBackground;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        buttonBackground = GetComponent<Image>();
    }

    private void Update()
    {
        if (gm.currentMana <= spellData.spellCost)
        {
            button.interactable = false;
            buttonBackground.color = Color.darkGray;
        } else
        {
            button.interactable = true;
            buttonBackground.color = Color.white;
        }
    }

    public void OnClick()
    {
        SpellManager.Instance.ActivateSpell(spellData);
    }
}