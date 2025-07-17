namespace WorkflowEngine.Models;
using System;
using System.Collections.Generic;

public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DefinitionId { get; set; } = default!;
    public string CurrentState { get; set; } = default!;
    public List<(string ActionId, DateTime Timestamp)> History { get; set; } = new();
}
