namespace WorkflowEngine.Models;
using System.Collections.Generic;

public class WorkflowDefinition
{
    public string Id { get; set; } = default!;
    public List<State> States { get; set; } = new();
    public List<WorkflowAction> Actions { get; set; } = new();
}
