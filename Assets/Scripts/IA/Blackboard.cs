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

    private List<float> _allMadness;
    private InGameHUD _hud;
    
    private void OnEnable()
    {
        foreach (var item in listSpriteWrapper)
            sprites.Add(item.direction, item.sprite);

        OnAddToBlackboard += AddToBlackboard;
        OnRemoveFromBlackboard += RemoveFromBlackboard;
    }
    
    private void Start() {
        _hud = InGameHUD.Instance;
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
        agentsToRemoveAtNextFrame.Add(agent);
    }

    public void UpdateAgentsList()
    {
        for (int i = 0; i < agentsToAddAtNextFrame.Count; i++)
        {
            AgentStateManager agent = agentsToAddAtNextFrame[i];

            if (!agents.Contains(agent))
            {
                agent.agentData.AssignSprites(ref sprites);
                agent.agentData.SetSprite(agent.agentData.lastDirection);
                agents.Add(agent);
                _hud.SetAdeptCounter(agents.Count, 66);
            }
        }

        for (int i = 0; i < agentsToRemoveAtNextFrame.Count; i++)
        {
            AgentStateManager agent = agentsToRemoveAtNextFrame[i];

            if (!agents.Contains(agent)) continue;

            agents.Remove(agent);
            
            if(agent)
                Destroy(agent.gameObject);
        }
        
        _allMadness = new List<float>();
        foreach (var agent in agents) {
            _allMadness.Add(agent.agentData.GetMadness());
        }
        _hud.SetMadness(_allMadness, 100);
    }
}
