using Unity.Burst.Intrinsics;
using UnityEngine;

public class ChangeRole : SingleSpell
{

    public override void UseSpell(IDamageable target)
    {
        GameObject go = ((MonoBehaviour)target).gameObject;
        AgentStateManager asm = go.GetComponent<AgentStateManager>();
        AgentData agentData = asm.agentData;

        if (gm.currentMana < data.spellCost)
            return;

        ConsumeMana(gm);
        Debug.Log($"J'ai consommé du mana pour changer de rôle");

        if (data.spellEffectPrefab)
        {
            GameObject fx = Instantiate(
                data.spellEffectPrefab,
                go.transform.position + Vector3.up,
                Quaternion.identity
            );
            Destroy(fx, 3f);
        }

        if (asm != null && agentData.role != data.roles)
        {
            agentData.role = data.roles; 
            Debug.Log($"Je viens de prendre le rôle de {data.roles}");
        }
    }
}

