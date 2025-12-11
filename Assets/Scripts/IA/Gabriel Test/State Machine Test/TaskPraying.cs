using UnityEngine;

public class TaskPraying : Task
{
    public float taskDuration = 5f;
    public float taskDetectionRadius = 4f;
    public ObjectType chatpelle = ObjectType.Chatpelle;
    
    
    #region Function to Use with TaskManager
    public override float GetPriority(AgentData agentData)
    {
        return 1f;
    }

    public override bool CanDoTask(AgentData agentData)
    {
        return true;
    }
    #endregion

    private void Pray()
    {
        // lance l'animation de pray
    }
    
    #region State Machine Basic Functions

    public override void OnStart(AgentStateManager agent)
    {
        agent.rangeToTarget = taskDetectionRadius;
        agent.timer = 0;
        agent.taskDuration = taskDuration;
        agent.isTaskFinished = false;

        if (!agent.FindNewTarget(chatpelle, mapData))
        {
            agent.isTaskFinished = true;
            return;
        }

        if (!agent.HasAgentReachedTarget())
        {
            Debug.Log("!agent.HasAgentReachedTarget()");
            agent.FindNewPath(mapData);
        }

    }

    public override void OnUpdate(AgentStateManager agent)
    {
        if (agent.isTaskFinished) return;
        if (agent.currentTarget == null)
        {
            if (!agent.FindNewTarget(chatpelle, mapData))
            {
                agent.isTaskFinished = true;
                return;
            }
        }

        if (agent.HasAgentReachedTarget())
        {
            agent.UpdateTimer();
            Pray();
            if (agent.isTimerFinished)
            {
                Debug.Log("Give Mana To Player");
                GameManager.Instance.currentMana += 5;

                Debug.Log("Give Madness To AI");
                float newMadness = agent.agentData.GetMadness() + 5f;
                agent.agentData.SetMadness(newMadness);

                agent.isTaskFinished = true;
            }
        }
        else
        {
            if (agent.pathNodes.Count == 0)
            {
                Debug.Log("agent.pathNodes.Count == 0");
                if (!agent.FindNewPath(mapData))
                {
                    agent.isTaskFinished = true;
                    return;
                }
            }
            
            if (!agent.MoveTowardPathNode())
            {
                Debug.Log("!agent.MoveTowardPathNode()");
                if (!agent.FindNewPath(mapData) && !agent.HasAgentReachedTarget())
                {
                    agent.isTaskFinished = true;
                    return;
                }
            }
        }
    }

    public override void OnStop(AgentStateManager agent)
    {
        agent.timer = 0;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
    }

    public override void OnCancel(AgentStateManager agent)
    {
        agent.timer = 0;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
    }
    
    #endregion
}
