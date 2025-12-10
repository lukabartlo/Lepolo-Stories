using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]

public class SpellData : ScriptableObject
{
    public int spellID;
    public float spellCost;
    public string spellName;
    //public Sprite icon;
    public SpellEnum type;
    public GameObject spellEffectPrefab;
    public Texture2D cursorTexture;
}



