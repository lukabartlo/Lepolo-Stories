using UnityEngine;

public class TaskEat : Task
{
    
    #region Function to Use ith TaskManager

    public override float GetPriority(AgentData agentData)
    {
        return priorityCurve.TestPriorityLevel(0 ,agentData.GetMaxHealth(),agentData.GetHealth());
    }

    public override bool CanDoTask(AgentData agentData)
    {
        return true;
    }
    
    #endregion
    
    #region State Machine Basic Functions

    public override void OnStart(AgentStateManager agent)
    {
        Debug.Log("Start Eat");
    }

    public override void OnUpdate(AgentStateManager agent)
    {
        
    }

    public override void OnStop(AgentStateManager agent)
    {
        
    }

    public override void OnCancel(AgentStateManager agent)
    {
        
    }
    
    #endregion
    
}
