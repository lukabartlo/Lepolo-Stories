using UnityEngine;

public class TaskIdle : Task
{
    [SerializeField] private Vector2Int minMaxRange =  new Vector2Int(3, 10); // correspond au nombre de déplacement
    [SerializeField] private float targetRange = 0.25f;
    [SerializeField] private Vector2Int minMaxWait =  new Vector2Int(2, 5); // correspond au nombre de déplacement
    
    
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

    private Vector3 GetARandomTargetPosition(AgentStateManager agent)
    { 
        int moveOpportunity = Random.Range(minMaxRange.x, minMaxRange.y);
        int dir =  Random.Range(0, 4);
        
        Vector3 target = agent.transform.position;
        target = GetATargetByDirection(target, dir, moveOpportunity);
        if (mapData.pathfinding.IsTargetWalkable(target) && mapData.IsCoordInMap(target)) return target;   
        
        while (!mapData.pathfinding.IsTargetWalkable(target))
        {
            for (int i = 0; i < 4; i++)
            {
                dir++;
                if (dir > 3) dir = 0;
                target = GetATargetByDirection(agent.transform.position, dir, moveOpportunity);
                if (mapData.pathfinding.IsTargetWalkable(target) && mapData.IsCoordInMap(target)) return target;
            }
            moveOpportunity++;
        }
        
        //Debug.Log("after while");
        return agent.transform.position;
    }

    private Vector3 GetATargetByDirection(Vector3 target, int direction, int moveOpportunity)
    {
        switch (direction)
        {
            case 0: // right
                return new Vector3(target.x + moveOpportunity, target.y, target.z);
            case 1: // down
                return new Vector3(target.x, target.y, target.z - moveOpportunity);
            case 2: // left
                return new Vector3(target.x - moveOpportunity, target.y, target.z);
            case 3: // up
                return new Vector3(target.x, target.y, target.z + moveOpportunity);
        } 
        //Debug.Log("after switch");
        return target;
    }
    
    #region State Machine Basic Functions

    public override void OnStart(AgentStateManager agent)
    {
        agent.timer = 0;
        agent.taskDuration = Random.Range(minMaxWait.x, minMaxWait.y);
        agent.rangeToTarget = targetRange;
        
        // chercher un endroit random ou aller 
        agent.targetPosition = GetARandomTargetPosition(agent);
        agent.FindNewPath(mapData, agent.targetPosition);
    }

    
    public override void OnUpdate(AgentStateManager agent)
    {
        if (agent.isTaskFinished) return;

        if (agent.HasAgentReachedTarget(agent.targetPosition))
        {
            agent.UpdateTimer();
            if (agent.isTimerFinished)
            {
                //Debug.Log("Give Mana To Player");
                
                agent.isTaskFinished = true;
            }
        }
        else
        {
            if (agent.pathNodes.Count == 0)
            {
                Debug.Log("agent.pathNodes.Count == 0");
                if (!agent.FindNewPath(mapData, agent.targetPosition))
                {
                    agent.isTaskFinished = true;
                    return;
                }
            }
            
            if (!agent.MoveTowardPathNode())
            {
                Debug.Log("!agent.MoveTowardPathNode()");
                if (!agent.FindNewPath(mapData, agent.targetPosition) && !agent.HasAgentReachedTarget(agent.targetPosition))
                {
                    agent.isTaskFinished = true;
                    return;
                }
            }
        }
    }


    public override void OnStop(AgentStateManager agent)
    {
        agent.isTaskFinished = false;
    }

    public override void OnCancel(AgentStateManager agent)
    {
        agent.isTaskFinished = false;
    }
    
    #endregion
    
}
