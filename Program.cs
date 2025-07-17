using WorkflowEngine.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using WorkflowEngine.Services;



var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var engine = new WorkflowEngineService();

app.MapPost("/definitions", (WorkflowDefinition def) =>
{
    if (engine.AddDefinition(def, out var msg))
        return Results.Ok(msg);
    return Results.BadRequest(msg);
});

app.MapGet("/definitions/{id}", (string id) =>
{
    var def = engine.GetDefinition(id);
    return def is not null ? Results.Ok(def) : Results.NotFound();
});

app.MapPost("/instances/start/{defId}", (string defId) =>
{
    var inst = engine.StartInstance(defId);
    return inst is not null ? Results.Ok(inst) : Results.BadRequest("Invalid definition ID");
});

app.MapPost("/instances/{id}/action/{actionId}", (string id, string actionId) =>
{
    if (engine.ExecuteAction(id, actionId, out var msg))
        return Results.Ok(msg);
    return Results.BadRequest(msg);
});

app.MapGet("/instances/{id}", (string id) =>
{
    var inst = engine.GetInstance(id);
    return inst is not null ? Results.Ok(inst) : Results.NotFound();
});

app.Run();
