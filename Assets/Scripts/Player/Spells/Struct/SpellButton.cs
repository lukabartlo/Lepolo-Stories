using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    [SerializeField] private SpellData spellData;
    [SerializeField] private GameManager gm;
    [SerializeField] private Button button;
    private Image buttonBackground;
    private Color offColor;
    private Color onColor;
    [SerializeField] private string offColorText;
    [SerializeField] private string onColorText;


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

            ColorUtility.TryParseHtmlString(offColorText, out offColor);

            ColorBlock colors = button.colors;
            colors.disabledColor = offColor;
            button.colors = colors;
        }
        else
        {
            button.interactable = true;

            ColorUtility.TryParseHtmlString(onColorText, out onColor);
            buttonBackground.color = onColor;
        }
    }


    public void OnClick()
    {
        SpellManager.Instance.ActivateSpell(spellData);
    }
}