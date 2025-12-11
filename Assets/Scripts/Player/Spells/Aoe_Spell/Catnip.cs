using Unity.Burst.Intrinsics;
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

        AgentStateManager asm = go.GetComponent<AgentStateManager>();
        if (asm != null)
        {
            AgentData agentData = asm.agentData;
            agentData.SetMadness(agentData.GetMadness() - 20f);
        }


    }
}
