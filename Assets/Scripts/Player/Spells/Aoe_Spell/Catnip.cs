using UnityEngine;

public class Catnip : AoeSpell
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

        go.GetComponent<EnnemyBehaviour>().currentMadness -= 50;


    }
}
