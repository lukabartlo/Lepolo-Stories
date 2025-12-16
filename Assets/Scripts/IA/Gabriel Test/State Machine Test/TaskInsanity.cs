using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskInsanity : Task
{
    [SerializeField] private Vector2Int _minMaxRange =  new Vector2Int(3, 10); // correspond au nombre de déplacement
    [SerializeField] private float _targetRange = 0.25f;
    [SerializeField] private float _insanityvalue = 0.15f;
    [SerializeField, Range(1,2)] private float _insanitySpeedMult = 1.5f;
    [SerializeField] private LayerMask _layerMask;
    

    #region Function to Use with TaskManager
    public override float GetPriority(AgentData agentData)
    {
        if(agentData.madness >= 100f)
        {
            
            return 1.2f;
        }
        
        return 0f;
    }

    public override bool CanDoTask(AgentData agentData)
    {
        return true;
    }
    #endregion

    private Vector3 GetARandomTargetPosition(AgentStateManager agent)
    { 
        int moveOpportunity = Random.Range(_minMaxRange.x, _minMaxRange.y);
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
        agent.rangeToTarget = _targetRange;
        agent.insanityMultiplier = _insanitySpeedMult;
        
        // chercher un endroit random ou aller 
        agent.targetPosition = GetARandomTargetPosition(agent);
        agent.FindNewPath(mapData, agent.targetPosition);
    }
    
    public override void OnUpdate(AgentStateManager agent)
    {
        if (agent.isTaskFinished) return;
        
        Collider[] colliders = Physics.OverlapSphere(agent.transform.position, 5f, _layerMask);
        
        foreach (Collider hitColliders in colliders)
        {
            if (hitColliders.gameObject == agent.gameObject)
                continue;
            
            hitColliders.GetComponent<AgentStateManager>().agentData.madness += _insanityvalue;
        }

        if (agent.HasAgentReachedTarget(agent.targetPosition))
        { 
            agent.isTaskFinished = true;
        }
        else
        {
            if (agent.pathNodes.Count == 0)
            {
                if (!agent.FindNewPath(mapData, agent.targetPosition))
                {
                    agent.isTaskFinished = true;
                    return;
                }
            }
            
            if (!agent.MoveTowardPathNode())
            {
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
