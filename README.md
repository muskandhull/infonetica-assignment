# Workflow Engine API - Infonetica Assignment

This project implements a lightweight **workflow engine** using **ASP.NET Core Minimal APIs**. It allows users to define workflows, start workflow instances, and trigger transitions via actions.



##  Features

- Define workflows with multiple states and actions
- Start new workflow instances
- Perform actions to transition between states
- Simple in-memory state management (no database)



##Project Structure

 Models/ → Data models (State, Action, etc.)
Services/ → Core engine logic
Program.cs → Main API setup
WorkflowEngine.csproj → Project file
