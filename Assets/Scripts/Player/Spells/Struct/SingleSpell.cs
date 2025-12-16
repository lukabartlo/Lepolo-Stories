using UnityEngine;

public abstract class SingleSpell : Spell
{
    public override void CastSpell(RaycastHit hit)
    {
        IDamageable target = hit.collider.GetComponent<IDamageable>();

        if (target == null)
            return;

        UseSpell(target);
    }
    protected abstract void UseSpell(IDamageable target);
}