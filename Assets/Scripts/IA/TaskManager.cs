using System;
using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    // Select Task by Roles
    [SerializeField] Dictionary<Roles,List<Task>> taskByRole;
    [SerializeField] List<Task> adepteList;
    [SerializeField] List<Task> guerrierList;
    [SerializeField] List<Task> pretreList;
    [SerializeField] List<Task> jehochatList;
    
    MapData mapData;
    bool isDictionarInitialized =  false;
    bool areTaskReady = false;

    
    // ref Blackboard
    public Blackboard blackboard;

    public void InitializeAllTask(MapData newMapData)
    {
        mapData = newMapData;
        InitializeDictionary();
        
        foreach (var tasks  in taskByRole)
        {
            foreach (var task in tasks.Value)
            {
                task.mapData = newMapData;
            }
        }
        areTaskReady = true;
        
        Debug.Log("InitializeAllTask");
    }

    private void InitializeDictionary()
    {
        taskByRole = new Dictionary<Roles, List<Task>>();
        taskByRole.Add(Roles.Adepte,adepteList);
        taskByRole.Add(Roles.Guerrier,guerrierList);
        taskByRole.Add(Roles.Pretre,pretreList);
        taskByRole.Add(Roles.Jehochat,jehochatList);
        
        isDictionarInitialized = true;
    }

    private void Update()
    {
        if (!isDictionarInitialized || !areTaskReady || !mapData.isMapGenerated) return;
        
        foreach (AgentStateManager _agent in blackboard.agents)
        {
            ExecuteTask(_agent);
        }
    }

    private Task GetHigherPriorityTask(AgentData data)
    {
        Task _newTask = null;
        float _bestPriority = -1;
        
        List<Task> _taskToCheck = taskByRole[data.role];
        
        foreach (Task _task in _taskToCheck)
        {
            if (!_task.CanDoTask(data)) continue;
            
            float _priority = _task.GetPriority(data);

            if (_priority > _bestPriority)
            {
                _bestPriority = _priority;
                _newTask = _task;
            }
        }

        return _newTask;
    }

    private void ExecuteTask(AgentStateManager _agent)
    {
        Task _priorisedTask = GetHigherPriorityTask(_agent.agentData);

        if (_agent.currentTask == null)
        {
            _agent.currentTask = _priorisedTask; // Set new Task
            _agent.currentTask.OnStart(_agent);
        }
        else if (_priorisedTask != null && _priorisedTask != _agent.currentTask) // If the Task has to change
        {
            _agent.currentTask.OnStop(_agent);
            _agent.currentTask = _priorisedTask; // Set new Task
                
            _agent.currentTask.OnStart(_agent);
        }
        else if(_priorisedTask != null && _agent.isTaskFinished)            // If the previous Task is Finished
        {
            _agent.currentTask.OnCancel(_agent);
            _agent.currentTask = _priorisedTask; // Set new Task
                
            _agent.currentTask.OnStart(_agent);
        }
        
        _agent.currentTask.OnUpdate(_agent);
    }
}
