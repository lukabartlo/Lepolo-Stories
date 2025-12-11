using System.Collections.Generic;
using UnityEngine;
using System;

public class Blackboard : MonoBehaviour
{
    public List<AgentStateManager> agents = new List<AgentStateManager>();
    public static Action<AgentStateManager> OnAddToBlackboard;
    public static Action<AgentStateManager> OnRemoveFromBlackboard;

    Dictionary<EDirection, Sprite> sprites = new();
    [SerializeField] private List<SpriteWrapper> listSpriteWrapper;

    private void OnEnable()
    {
        foreach (var item in listSpriteWrapper)
            sprites.Add(item.direction, item.sprite);

        OnAddToBlackboard += AddToBlackboard;
        OnRemoveFromBlackboard += RemoveFromBlackboard;
    }

    private void OnDisable()
    {
        
        OnAddToBlackboard -= AddToBlackboard;
        OnRemoveFromBlackboard -= RemoveFromBlackboard;
    }

    private void AddToBlackboard(AgentStateManager agent)
    {
        if (!agents.Contains(agent))
        {
            agent.agentData.AssignSprites(ref sprites, agent.GetComponentInChildren<SpriteRenderer>());
            agents.Add(agent);
        }
    }

    private void RemoveFromBlackboard(AgentStateManager agent)
    {
        agents.Remove(agent);
    }
}
