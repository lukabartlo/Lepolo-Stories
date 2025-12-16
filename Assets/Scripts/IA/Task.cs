using System;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public string taskName;
    
    public MapData mapData;
    
    public void GetPathFindingRef(MapData data)
    {
        mapData = data;
    }

    public TestCurve priorityCurve;
    
    #region Function to Use with TaskManager
    public abstract float GetPriority(AgentData agentData);
    public abstract bool CanDoTask(AgentData agentData); // link to the progression system

    #endregion
    
    
    #region State Machine Basic Functions
    
    // For the function bellow , use the variables in AgentStateManager
    public abstract void OnStart(AgentStateManager agent);
    public abstract void OnUpdate(AgentStateManager agent);
    public abstract void OnStop(AgentStateManager agent);
    public abstract void OnCancel(AgentStateManager agent);
    
    
    #endregion
    
    
}
