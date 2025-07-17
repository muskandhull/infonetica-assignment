using WorkflowEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowEngine.Services;

public class WorkflowEngineService
{
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    public bool AddDefinition(WorkflowDefinition def, out string message)
    {
        if (_definitions.ContainsKey(def.Id))
        {
            message = "Duplicate definition ID.";
            return false;
        }

        if (def.States.Count(s => s.IsInitial) != 1)
        {
            message = "Exactly one initial state required.";
            return false;
        }

        message = "Workflow added.";
        _definitions[def.Id] = def;
        return true;
    }

    public WorkflowDefinition? GetDefinition(string id) => 
        _definitions.TryGetValue(id, out var def) ? def : null;

    public WorkflowInstance? GetInstance(string id) => 
        _instances.TryGetValue(id, out var inst) ? inst : null;

    public WorkflowInstance? StartInstance(string defId)
    {
        if (!_definitions.TryGetValue(defId, out var def))
            return null;

        var initState = def.States.First(s => s.IsInitial);
        var instance = new WorkflowInstance
        {
            DefinitionId = defId,
            CurrentState = initState.Id
        };

        _instances[instance.Id] = instance;
        return instance;
    }

    public bool ExecuteAction(string instanceId, string actionId, out string message)
    {
        if (!_instances.TryGetValue(instanceId, out var instance))
        {
            message = "Instance not found.";
            return false;
        }

        var def = _definitions[instance.DefinitionId];
        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);

        if (action == null)
        {
            message = "Action not found.";
            return false;
        }

        if (!action.Enabled)
        {
            message = "Action disabled.";
            return false;
        }

        if (!action.FromStates.Contains(instance.CurrentState))
        {
            message = "Action not allowed from current state.";
            return false;
        }

        var toState = def.States.FirstOrDefault(s => s.Id == action.ToState && s.Enabled);
        if (toState == null)
        {
            message = "Invalid or disabled target state.";
            return false;
        }

        if (def.States.First(s => s.Id == instance.CurrentState).IsFinal)
        {
            message = "Instance is in a final state.";
            return false;
        }

        instance.CurrentState = toState.Id;
        instance.History.Add((actionId, DateTime.UtcNow));

        message = "Action executed.";
        return true;
    }

    public IEnumerable<WorkflowDefinition> GetAllDefinitions() => _definitions.Values;
    public IEnumerable<WorkflowInstance> GetAllInstances() => _instances.Values;
}
