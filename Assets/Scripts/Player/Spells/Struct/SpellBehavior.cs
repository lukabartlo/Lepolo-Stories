using UnityEngine;

public abstract class SpellBehavior : MonoBehaviour
{
    public SpellData data;
    
    protected GameManager gm;

    protected virtual void Awake()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    public abstract void UseSpell(IDamageable target);
    public abstract void CastSpell(RaycastHit hit);
    public virtual void ConsumeMana(GameManager gm)
    {
        gm.currentMana -= data.spellCost;
    }
}