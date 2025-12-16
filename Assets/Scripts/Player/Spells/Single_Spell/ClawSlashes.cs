using UnityEngine;

public class ClawSlashes : SingleSpell
{
    protected override void UseSpell(IDamageable target)
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
        AgentStateManager agentStateManager = go.GetComponent<AgentStateManager>();
        Debug.Log(agentStateManager);
        Blackboard.OnRemoveFromBlackboard?.Invoke(agentStateManager);
        Debug.Log(agentStateManager);
    }
}