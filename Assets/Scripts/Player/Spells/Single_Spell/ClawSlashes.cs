using UnityEngine;

public class ClawSlashes : SingleSpell
{
    public override void UseSpell(IDamageable target)
    {
        GameObject go = ((MonoBehaviour)target).gameObject;

        if (gm.currentMana < data.spellCost)
            return;

        ConsumeMana(gm);

        if (data.spellEffectPrefab)
        {
            GameObject fx = Instantiate(
                data.spellEffectPrefab,
                go.transform.position + Vector3.up,
                Quaternion.identity
            );
            Destroy(fx, 3f);
        }

        Destroy(go);
    }
}