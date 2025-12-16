using Unity.Burst.Intrinsics;
using UnityEngine;

public class ChangeRole : SingleSpell
{
    protected override void UseSpell(IDamageable target)
    {
        GameObject go = ((MonoBehaviour)target).gameObject;
        AgentStateManager asm = go.GetComponent<AgentStateManager>();
        AgentData agentData = asm.agentData;

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

        if (asm != null && agentData.role != data.roles)
        {
            agentData.spriteRendererRef.sprite = data.roleSprite;
            agentData.role = data.roles; 
            
        }
    }
}

