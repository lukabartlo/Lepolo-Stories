using UnityEngine;

public class TaskGatherAdepts : Task
{
    // Duration for fade in/out animations
    public float fadeDuration = 2f;
    
    // Prefab of the Adept agent to spawn
    public GameObject adeptPrefab;
    
    // Spawn radius around the Jehochat
    public float spawnRadius = 3f;
    
    // Cooldown duration before the task can be performed again (in seconds)
    public float cooldownDuration = 2f;
    
    private GatheringPhase _currentPhase;
    
    private float _minorPhaseDuration;
    private float _expeditionDuration = 15f;
    
    #region Function to Use with TaskManager
    
    public override float GetPriority(AgentData agentData)
    {
        // Higher priority when available
        return 1f;
    }

    public override bool CanDoTask(AgentData agentData)
    {
        // Only Jehochat can perform this task
        if (agentData.role != Roles.Jehochat)
        {
            return false;
        }

        return true;
    }
    
    #endregion
    
    /// <summary>
    /// Spawns a random number of Adept agents around the Jehochat
    /// </summary>
    private void SpawnAdepts(AgentStateManager agent)
    {
        // Random number of adepts between 1 and 5
        int adeptCount = Random.Range(1, 6);
        
        for (int i = 0; i < adeptCount; i++)
        {
            // Calculate random position around the Jehochat
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(1f, spawnRadius);
            
            Vector3 spawnOffset = new Vector3(
                Mathf.Cos(angle) * distance,
                0f,
                Mathf.Sin(angle) * distance
            );
            
            Vector3 spawnPosition = agent.transform.position + spawnOffset;
            if (!mapData.IsCoordInMap(spawnPosition)) spawnPosition = agent.transform.position;
            
            // Instantiate the Adept prefab if available
            if (adeptPrefab != null)
            {
                GameObject newAdept = Object.Instantiate(adeptPrefab, spawnPosition, Quaternion.identity);
                
                // Optional: Set the Adept's role through AgentStateManager if it exists
                AgentStateManager adeptAgent = newAdept.GetComponent<AgentStateManager>();
                if (adeptAgent != null && adeptAgent.agentData != null)
                {
                    adeptAgent.agentData.role = Roles.Adepte;
                }
            }
        }
    }
    
    /// <summary>
    /// Updates the alpha value of the Jehochat's materials for fade effect
    /// </summary>
    private void UpdateJehochatAlpha(AgentStateManager agent ,float alpha)
    {
        agent.agentData.SetNewAlpha(alpha);
    }

    private Vector3 GetClosestMapBorder(AgentStateManager agent)
    {
        Vector3 closestMapBorder = Vector3.positiveInfinity;
        Vector3 posChecked;
        float closestDistance = float.MaxValue;
        Transform agentTransform = agent.transform;
        
        Vector2 maxXY = mapData.GetMaxXY();

        for (int direction = 0; direction < 4; direction++)
        {
            switch (direction)
            {
                case 0: // right
                    posChecked = new Vector3(maxXY.x, agentTransform.position.y, agentTransform.position.z);
                    break;
                case 1: // down
                    posChecked = new Vector3(agentTransform.position.x, agentTransform.position.y, 1);
                    break;
                case 2: // left
                    posChecked = new Vector3(1 ,agentTransform.position.y, agentTransform.position.z);
                    Vector3.Distance(agentTransform.position, new Vector3(0,agentTransform.position.y, agentTransform.position.z));
                    break;
                case 3: // up
                    posChecked = new Vector3(agentTransform.position.x, agentTransform.position.y, maxXY.y);
                    break;
                default:
                    posChecked = agentTransform.position;
                    break;
            }
            
            float disChecked = Vector3.Distance(agentTransform.position, posChecked);
            if (disChecked < closestDistance)
            {
                closestMapBorder = posChecked;
                closestDistance = disChecked;
            }
        }

        return closestMapBorder;
    }
    
    #region State Machine Basic Functions
    
    public override void OnStart(AgentStateManager agent)
    {
        //Debug.Log("Start the Task of the JeoChat");
        // Initialize the gathering ritual
        agent.actualGatheringPhase = GatheringPhase.FadingOut;
        agent.timer = 0f;
        agent.taskDuration = fadeDuration;
        agent.doIdleAfterTask = true;

        agent.originalColor = agent.agentData.spriteRendererRef.color;
        agent.originalAlpha = agent.originalColor.a;

        agent.targetPosition = GetClosestMapBorder(agent);
        Debug.Log($"jehochat pos : {agent.transform.position} , {agent.targetPosition}");
        agent.FindNewPath(mapData, agent.targetPosition); // aller a cette position
        Debug.Log($"jehochat pathnode count : {agent.pathNodes.Count}");
    }

    public override void OnUpdate(AgentStateManager agent)
    {
        if (agent.isTaskFinished) return;

        float fadeOutProgress;
        float currentAlpha;
        
        switch (agent.actualGatheringPhase)
        {
            case GatheringPhase.FadingOut:
                if (agent.HasAgentReachedTarget(agent.targetPosition))
                {
                    if (!agent.isPlayingSound)
                    {
                        agent.isPlayingSound = true;
                        SoundManager.OnSoundSpatializedPlayed?.Invoke(SoundName.JehoChatLeaving, agent.audioSource); // lance le son une première fois
                    }
                    
                    // Gradually fade out the Jehochat
                    agent.timer += Time.deltaTime; 
                    fadeOutProgress = Mathf.Clamp01(agent.timer / fadeDuration);
                    currentAlpha = Mathf.Lerp(agent.originalAlpha, 0f, fadeOutProgress);
                    UpdateJehochatAlpha(agent,currentAlpha);
                
                    if (fadeOutProgress >= 1f)
                    {
                        agent.audioSource.Stop(); // arrete le son
                        agent.isPlayingSound = false;
                    
                        // Transition to waiting phase
                        agent.actualGatheringPhase = GatheringPhase.Waiting;
                        agent.timer = 0f;
                        agent.taskDuration = _expeditionDuration;
                    }
                }
                else
                {
                    if (agent.pathNodes.Count == 0)
                    {
                        Debug.Log("jehochat : agent.pathNodes.Count == 0");
                        if (!agent.FindNewPath(mapData, agent.targetPosition))
                        {
                            return;
                        }
                    }
            
                    if (!agent.MoveTowardPathNode())
                    {
                        Debug.Log("jehochat : !agent.MoveTowardPathNode()");
                        if (!agent.FindNewPath(mapData, agent.targetPosition) && !agent.HasAgentReachedTarget(agent.targetPosition))
                        {
                            return;
                        }
                    }
                }
                break;
            
            case GatheringPhase.Waiting:
                // Wait for the task duration
                agent.UpdateTimer();
                
                if (agent.isTimerFinished)
                {
                    // Transition to fading in phase
                    agent.actualGatheringPhase = GatheringPhase.FadingIn;
                    agent.timer = 0f;
                    agent.taskDuration = fadeDuration;
                    agent.isTimerFinished = false;
                }
                break;
                
            case GatheringPhase.FadingIn:
                
                if (!agent.isPlayingSound)
                {
                    agent.isPlayingSound = true;
                    SoundManager.OnSoundSpatializedPlayed?.Invoke(SoundName.JehoChatComingBack, agent.audioSource); // lance le son une première fois
                }
                // Gradually fade in the Jehochat
                agent.timer += Time.deltaTime;
                float fadeInProgress = Mathf.Clamp01(agent.timer / fadeDuration);
                currentAlpha = Mathf.Lerp(0f, agent.originalAlpha, fadeInProgress);
                UpdateJehochatAlpha(agent,currentAlpha);
                
                if (fadeInProgress >= 1f)
                {
                    agent.audioSource.Stop(); // arrete le son
                    agent.isPlayingSound = false;
                    
                    // Transition to spawning phase
                    agent.actualGatheringPhase = GatheringPhase.SpawningAdepts;
                }
                break;
                
            case GatheringPhase.SpawningAdepts:
                // Spawn the Adept agents
                SpawnAdepts(agent);
                
                // Mark task as completed
                agent.actualGatheringPhase = GatheringPhase.Completed;
                agent.timer = 0f;
                agent.taskDuration = cooldownDuration;
                agent.isTimerFinished = false;
                break;
                
            case GatheringPhase.Completed:
                // Task is finished
                agent.UpdateTimer();
                if (agent.isTimerFinished)
                {
                    agent.isTaskFinished = true;
                }
                
                break;
        }
    }

    public override void OnStop(AgentStateManager agent)
    {
        // Reset values and restore original alpha
        agent.timer = 0;
        agent.isPlayingSound =  false;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
        
        _currentPhase = GatheringPhase.FadingOut;

         UpdateJehochatAlpha(agent ,agent.originalAlpha);
    }

    public override void OnCancel(AgentStateManager agent)
    {
        // Reset values and restore original alpha
        agent.timer = 0;
        agent.isPlayingSound =  false;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
        _currentPhase = GatheringPhase.FadingOut;

        UpdateJehochatAlpha(agent, agent.originalAlpha);
    }
    
    #endregion
}
