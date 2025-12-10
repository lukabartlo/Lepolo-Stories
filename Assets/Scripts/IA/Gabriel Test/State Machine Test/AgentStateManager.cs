using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentStateManager : MonoBehaviour
{
    public AgentData agentData;

    #region Variables for Task

    public Task currentTask;
    public bool isTaskFinished = false;

    [Header("For movement")] 
    private Transform agentTransform;
    public Transform currentTarget;
    
    
    public List<PathNode> pathNodes = new List<PathNode>();
    public List<Vector2Int> pathNodesExcluded = new List<Vector2Int>();
    
    
    public int pathNodeIndex = 0;
    public float rangeToTarget = 1f;
    public bool isTargetReached = false;
    private Rigidbody rb;
    public float speed = 5f;
    
    [Space(20)] [Header("For Timer")]
    public float timer = 0;
    public float taskDuration = 1f;
    public bool isTimerFinished = false;
    
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agentTransform =  GetComponent<Transform>();
    }

    private void OnEnable()
    {
        AddToBlackboard();
    }
    private void OnDisable()
    {
        RemoveFromBlackboard();
    }

    void AddToBlackboard()
    {
        Blackboard.OnAddToBlackboard?.Invoke(this);
    }
    void RemoveFromBlackboard()
    {
        Blackboard.OnRemoveFromBlackboard?.Invoke(this);
    }

    public bool HasAgentReachedTarget()
    {
        isTargetReached = (Vector3.Distance(currentTarget.position, agentTransform.position) <= rangeToTarget);
        return isTargetReached;
    }

    public void MoveTowardPathNode(PathNode target)
    {
        Vector3 targetPos = new Vector3(target.X, agentTransform.position.y, target.Y); // get direction
        Vector3 direction = (targetPos - agentTransform.position).normalized;
        
        rb.linearVelocity = speed * Time.deltaTime * direction;

        if (Vector3.Distance(agentTransform.position, targetPos) < 0.25f) pathNodeIndex++;
    }

    public void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer >= taskDuration)
        {
            timer -= taskDuration;
            isTimerFinished = true;
            return;
        }
        isTimerFinished = false;
    }
    
    public bool FindNewPath(MapData mapData)
    {
        pathNodes = mapData.pathfinding.FindPath(agentTransform.position, currentTarget.position);
        pathNodesExcluded = mapData.GetConnectedCellsFull((int)currentTarget.position.x, (int)currentTarget.position.z);
        pathNodeIndex = 0;
        return pathNodes.Count > 0;
    }

    public bool FindNewTarget(ObjectType objectType, MapData mapData)
    {
        GameObject target;
        if ((target = mapData.GetClosestMapObject(agentTransform.position, objectType)) != null)
        {
            currentTarget = target.transform;
        }
        
        return target != null;
    }

    private void OnDrawGizmos()
    {
        if (currentTarget == null ) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentTarget.position, rangeToTarget);
    }
}
