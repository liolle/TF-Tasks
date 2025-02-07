using apiExo.api.Models;
using apiExo.CQS;
using apiExo.domain.Commands;
using apiExo.domain.entity;
using apiExo.domain.Queries;
using apiExo.domain.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiExo.controllers;

public class TaskController(ITaskService _ts): ControllerBase
{
    private int _userId;

    [HttpGet]
    [Authorize]
    public ActionResult All()
    {
        SetUser();
        if (_userId  == 0){
            return BadRequest(ICommandResult.Failure("Malformed Token"));
        }
        AllTaskQuery query = new()
        {
            Id = _userId
        };

        var tasks = _ts.Execute(query);
        return Ok(tasks);
    }


    [HttpGet]
    [Authorize]
    public ActionResult GetById([FromQuery] int id )
    {
        SetUser();
        if (_userId  == 0){
            return BadRequest(ICommandResult.Failure("Malformed Token"));
        }

        TaskByIdQuery query = new()
        {
            TaskId = id,
            UserId = _userId
        };

        var task = _ts.Execute(query);
        return task != null ? Ok(task) : NotFound($"Task with ID {query.TaskId} not found.");
    }

    [HttpPost]
    [Authorize]
    public ActionResult Add([FromBody] AddTaskModel model)
    {
        SetUser();
        if (_userId  == 0){
            return BadRequest(ICommandResult.Failure("Malformed Token"));
        }

        AddTaskCommand command = new(
            model.Title,
            model.Status,
            _userId
        );

        return Ok(_ts.Execute(command));
    }

    [HttpPut]
    [Authorize]
    public ActionResult Update([FromBody] UpdateTaskModel model)
    {
        SetUser();
        if (_userId  == 0){
            return BadRequest(ICommandResult.Failure("Malformed Token"));
        }

        UpdateTaskCommand command = new(
            model.Title,
            model.Status,
            _userId,
            model.Id
        );

        return Ok(_ts.Execute(command));
    }


    [HttpPatch]
    [Authorize]
    public ActionResult Patch([FromBody] PatchTaskModel model)
    {
        SetUser();
        if (_userId  == 0){
            return BadRequest(ICommandResult.Failure("Malformed Token"));
        }

        PatchTaskCommand command = new(
            model.Status,
            model.Id,
            _userId
        );

        return Ok(_ts.Execute(command));
    }


    void SetUser(){
        string? userId = User.FindFirst(nameof(ApplicationUser.Id))?.Value;
        int.TryParse(userId, out _userId);
    }
}