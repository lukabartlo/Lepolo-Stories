using System.Collections.Generic;
using UnityEngine;
using System;

public class Blackboard : MonoBehaviour
{
    public List<AgentStateManager> agents = new List<AgentStateManager>();
    public List<AgentStateManager> agentsToAddAtNextFrame = new List<AgentStateManager>();
    public List<AgentStateManager> agentsToRemoveAtNextFrame = new List<AgentStateManager>();
    
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
        if (!agentsToAddAtNextFrame.Contains(agent))
            agentsToAddAtNextFrame.Add(agent);
    }

    private void RemoveFromBlackboard(AgentStateManager agent)
    {
        agentsToRemoveAtNextFrame.Remove(agent);
    }

    public void UpdateAgentsList()
    {
        foreach (AgentStateManager agent in agentsToAddAtNextFrame)
        {
            if (!agents.Contains(agent))
            {
                agent.agentData.AssignSprites(ref sprites);
                agents.Add(agent);
            }
        }

        foreach (AgentStateManager agent in agentsToRemoveAtNextFrame)
        {
            if (agents.Contains(agent))
                agents.Remove(agent);
        }
    }
}
