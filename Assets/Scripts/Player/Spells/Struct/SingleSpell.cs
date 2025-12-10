using UnityEngine;

public abstract class SingleSpell : SpellBehavior
{
    public override void CastSpell(RaycastHit hit)
    {
        IDamageable target = hit.collider.GetComponent<IDamageable>();

        if (target == null)
            return;

        UseSpell(target);
    }
}