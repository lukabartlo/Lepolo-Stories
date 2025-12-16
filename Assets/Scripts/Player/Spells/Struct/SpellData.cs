using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]

public class SpellData : ScriptableObject
{
    // Identification
    [Header("Identification")]
    [Tooltip("Unique ID of the spell")]
    public int spellID;

    [Tooltip("Displayed name of the spell")]
    public string spellName;

    [Tooltip("Type/category of the spell")]
    public SpellEnum type;

    // Cost & Resources
    [Header("Cost & Resources")]
    [Tooltip("Resource cost to cast the spell")]
    public float spellCost;

    [Tooltip("Modification Value of Madness by the spell")]
    public float madnessValue;


    // Visuals
    [Header("Visuals")]
    [Tooltip("Prefab spawned when the spell is cast")]
    public GameObject spellEffectPrefab;

    [Tooltip("Cursor texture used when targeting")]
    public Texture2D cursorTexture;

    //AOE
    [Header("Area of Effect")]
    [Tooltip("Radius of the AOE")]
    public float aoeRadius;

    [Tooltip("Preview prefab shown before casting")]
    public GameObject aoePreviewPrefab;

    // Role
    [Header("Change Role")]
    [Tooltip("Assigned another Role when this spell is used")]
    public Roles roles;
}



