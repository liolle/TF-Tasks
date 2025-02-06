using apiExo.api.Models;
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
        setUser();
        if (_userId  == 0){
            return BadRequest("Malformed Token");
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
        setUser();
        if (_userId  == 0){
            return BadRequest("Malformed Token");
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
        setUser();
        if (_userId  == 0){
            return BadRequest("Malformed Token");
        }

        AddTaskCommand command = new(
            model.Title,
            model.Status,
            _userId
        );

        string result = _ts.Execute(command);
        return Ok(result);
    }

    [HttpPut]
    [Authorize]
    public ActionResult Update([FromBody] UpdateTaskModel model)
    {
        setUser();
        if (_userId  == 0){
            return BadRequest("Malformed Token");
        }

        UpdateTaskCommand command = new(
            model.Title,
            model.Status,
            _userId,
            model.Id
        );

        string result = _ts.Execute(command);
        return Ok(result);
    }


    [HttpPatch]
    [Authorize]
    public ActionResult Patch([FromBody] PatchTaskModel model)
    {
        setUser();
        if (_userId  == 0){
            return BadRequest("Malformed Token");
        }

        PatchTaskCommand command = new(
            model.Status,
            model.Id,
            _userId
        );

        string result = _ts.Execute(command);
        return Ok(result);
    }


    void setUser(){
        string? userId = User.FindFirst(nameof(ApplicationUser.Id))?.Value;
        int.TryParse(userId, out _userId);
    }
}