using UnityEngine;
using UnityEngine.Rendering;

public class TaskCuttingTree : Task
{
    public float taskDuration = 5f;
    public float taskDetectionRadius = 2f;
    public ObjectType tree = ObjectType.Tree;
    
    
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

    private void PlayCuttingAnimation()
    {
        // lance l'animation de pray
    }

    private void CutTree(GameObject target)
    {
        mapData.TryDeleteCellByObject(target);
    }
    
    #region State Machine Basic Functions

    public override void OnStart(AgentStateManager agent)
    {
        agent.rangeToTarget = taskDetectionRadius;
        agent.timer = 0;
        agent.taskDuration = taskDuration;
        agent.isTaskFinished = false;
        agent.doIdleAfterTask = true;

        if (!agent.FindNewTarget(tree, mapData))
        {
            agent.isTaskFinished = true;
            return;
        }

        if (!agent.HasAgentReachedTarget(agent.currentTarget.position))
        {
            //Debug.Log("!agent.HasAgentReachedTarget()");
            agent.FindNewPath(mapData, agent.currentTarget.position);
        }

    }

    public override void OnUpdate(AgentStateManager agent)
    {
        if (agent.isTaskFinished) return;
        if (agent.currentTarget == null)
        {
            if (!agent.FindNewTarget(tree, mapData))
            {
                agent.isTaskFinished = true;
                return;
            }
        }

        if (agent.HasAgentReachedTarget(agent.currentTarget.position))
        {
            agent.UpdateTimer();

            if (!agent.isPlayingSound)
            {
                agent.isPlayingSound = true;
                SoundManager.OnSoundSpatializedPlayed?.Invoke(SoundName.CuttingTree, agent.audioSource); // lance le son une première fois
            }
            
            if (agent.isTimerFinished)
            {
                agent.audioSource.Stop(); // arrete le son
                agent.isPlayingSound = false;
                CutTree(agent.currentTarget.gameObject);
                agent.isTaskFinished = true;
            }
        }
        else
        {
            if (agent.pathNodes.Count == 0)
            {
                //Debug.Log("agent.pathNodes.Count == 0");
                if (!agent.FindNewPath(mapData, agent.currentTarget.position))
                {
                    agent.isTaskFinished = true;
                    return;
                }
            }

            if (!agent.MoveTowardPathNode())
            {
                //Debug.Log("!agent.MoveTowardPathNode()");
                if (!agent.FindNewPath(mapData, agent.currentTarget.position) && !agent.HasAgentReachedTarget(agent.currentTarget.position))
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
        agent.isPlayingSound =  false;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
    }

    public override void OnCancel(AgentStateManager agent)
    {
        agent.timer = 0;
        agent.isPlayingSound =  false;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
    }
    
    #endregion
}
