using UnityEngine;

public abstract class SpellBehavior : MonoBehaviour
{
    public SpellData data;
    public abstract void CastSpell(GameObject target);
    public virtual void ConsumeMana(GameManager gm)
    {
        gm.currentMana -= data.spellCost;
    }
}