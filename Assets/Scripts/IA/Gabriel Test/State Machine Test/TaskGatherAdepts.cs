using UnityEngine;

public class TaskGatherAdepts : Task
{
    // Duration of the gathering ritual
    public float taskDuration = 10f;
    
    // Duration for fade in/out animations
    public float fadeDuration = 2f;
    
    // Prefab of the Adept agent to spawn
    public GameObject adeptPrefab;
    
    // Spawn radius around the Jehochat
    public float spawnRadius = 3f;
    
    // Cooldown duration before the task can be performed again (in seconds)
    public float cooldownDuration = 2f;
    
    // Time when the task was last completed
    private float _lastCompletionTime = -999f;
    
    // Enum to track the current phase of the task
    private enum GatheringPhase
    {
        FadingOut,
        Waiting,
        FadingIn,
        SpawningAdepts,
        Completed
    }
    
    private GatheringPhase _currentPhase;
    private float _phaseTimer;
    private Renderer _jehochatRenderer;
    private Color _originalColor;
    private float _originalAlpha;
    
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
        
        // Check if the cooldown has elapsed since the last completion
        float timeSinceLastCompletion = Time.time - _lastCompletionTime;
        return timeSinceLastCompletion >= cooldownDuration;
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
    private void UpdateJehochatAlpha(float alpha)
    {
        if (_jehochatRenderer == null) return;
        
        // Update all materials
        foreach (Material mat in _jehochatRenderer.materials)
        {
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
            
            // Enable transparency if needed
            if (alpha < 1f)
            {
                mat.SetFloat("_Mode", 3); // Transparent mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }
    
    #region State Machine Basic Functions
    
    public override void OnStart(AgentStateManager agent)
    {
        // Initialize the gathering ritual
        _currentPhase = GatheringPhase.FadingOut;
        _phaseTimer = 0f;
        
        // Get the renderer component
        _jehochatRenderer = agent.GetComponent<Renderer>();
        
        if (_jehochatRenderer != null && _jehochatRenderer.materials.Length > 0)
        {
            _originalColor = _jehochatRenderer.materials[0].color;
            _originalAlpha = _originalColor.a;
        }
        else
        {
            _originalAlpha = 1f;
        }
    }

    public override void OnUpdate(AgentStateManager agent)
    {
        if (agent.isTaskFinished) return;
        
        switch (_currentPhase)
        {
            case GatheringPhase.FadingOut:
                // Gradually fade out the Jehochat
                _phaseTimer += Time.deltaTime;
                float fadeOutProgress = Mathf.Clamp01(_phaseTimer / fadeDuration);
                float currentAlpha = Mathf.Lerp(_originalAlpha, 0f, fadeOutProgress);
                UpdateJehochatAlpha(currentAlpha);
                
                if (fadeOutProgress >= 1f)
                {
                    // Transition to waiting phase
                    _currentPhase = GatheringPhase.Waiting;
                    _phaseTimer = 0f;
                    agent.timer = 0f;
                }
                break;
                
            case GatheringPhase.Waiting:
                // Wait for the task duration
                agent.UpdateTimer();
                
                if (agent.isTimerFinished)
                {
                    // Transition to fading in phase
                    _currentPhase = GatheringPhase.FadingIn;
                    _phaseTimer = 0f;
                }
                break;
                
            case GatheringPhase.FadingIn:
                // Gradually fade in the Jehochat
                _phaseTimer += Time.deltaTime;
                float fadeInProgress = Mathf.Clamp01(_phaseTimer / fadeDuration);
                currentAlpha = Mathf.Lerp(0f, _originalAlpha, fadeInProgress);
                UpdateJehochatAlpha(currentAlpha);
                
                if (fadeInProgress >= 1f)
                {
                    // Transition to spawning phase
                    _currentPhase = GatheringPhase.SpawningAdepts;
                }
                break;
                
            case GatheringPhase.SpawningAdepts:
                // Spawn the Adept agents
                SpawnAdepts(agent);
                
                // Record the completion time for cooldown tracking
                _lastCompletionTime = Time.time;
                
                // Mark task as completed
                _currentPhase = GatheringPhase.Completed;
                agent.isTaskFinished = true;
                break;
                
            case GatheringPhase.Completed:
                // Task is finished
                break;
        }
    }

    public override void OnStop(AgentStateManager agent)
    {
        // Reset values and restore original alpha
        agent.timer = 0;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
        _phaseTimer = 0f;
        _currentPhase = GatheringPhase.FadingOut;
        
        if (_jehochatRenderer != null)
        {
            UpdateJehochatAlpha(_originalAlpha);
        }
    }

    public override void OnCancel(AgentStateManager agent)
    {
        // Reset values and restore original alpha
        agent.timer = 0;
        agent.currentTarget = null;
        agent.isTaskFinished = false;
        _phaseTimer = 0f;
        _currentPhase = GatheringPhase.FadingOut;
        
        if (_jehochatRenderer != null)
        {
            UpdateJehochatAlpha(_originalAlpha);
        }
    }
    
    #endregion
}
