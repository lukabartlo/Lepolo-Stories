using UnityEngine;

public class TaskIdle : Task
{
    
    #region Function to Use with TaskManager
    public override float GetPriority(AgentData agentData)
    {
        return 0.7f;
    }

    public override bool CanDoTask(AgentData agentData)
    {
        return true;
    }
    #endregion
    
    #region State Machine Basic Functions

    public override void OnStart(AgentStateManager agent)
    {
        Debug.Log("Start Idle");
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
