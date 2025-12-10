using UnityEngine;

public abstract class AoeSpell : SpellBehavior
{
    private float radius = 5f;

    public override void CastSpell(RaycastHit hit)
    {
        Collider[] hits = Physics.OverlapSphere(hit.point, radius);

        foreach (Collider c in hits)
        {
            IDamageable target = c.GetComponent<IDamageable>();

            if (target != null)
                UseSpell(target);
        }
    }
}