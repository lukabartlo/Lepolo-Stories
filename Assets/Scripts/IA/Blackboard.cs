using System.Collections.Generic;
using UnityEngine;
using System;

public class Blackboard : MonoBehaviour
{
    public List<AgentStateManager> agents = new List<AgentStateManager>();
    public static Action<AgentStateManager> OnAddToBlackboard;
    public static Action<AgentStateManager> OnRemoveFromBlackboard;

    private void OnEnable()
    {
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
            agents.Add(agent);
    }

    private void RemoveFromBlackboard(AgentStateManager agent)
    {
        agents.Remove(agent);
    }
}
