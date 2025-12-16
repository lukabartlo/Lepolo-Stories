using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellData data;
    
    protected GameManager gm;

    protected virtual void Awake()
    {
        gm = GameManager.Instance;
    }
    public abstract void CastSpell(RaycastHit hit);
    public virtual void ConsumeMana(GameManager gm)
    {
        gm.currentMana -= data.spellCost;
    }
}