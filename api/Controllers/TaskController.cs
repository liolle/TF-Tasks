using apiExo.bll.entity;
using apiExo.bll.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiExo.controllers;


public class TaskController(ITaskService _ts): ControllerBase
{
    private ApplicationUser? _applicationUser;

    [HttpGet]
    [Authorize]
    public ActionResult All()
    {
        setUser();
        if (_applicationUser is null){
            return BadRequest("Malformed Token");
        }
        var tasks = _ts.GetAll(_applicationUser.Id);
        return Ok(tasks);
    }


    [HttpGet]
    [Authorize]
    public ActionResult GetById([FromQuery] int id)
    {
        setUser();
        if (_applicationUser is null){
            return BadRequest("Malformed Token");
        }
        var task = _ts.GetByID(id,_applicationUser.Id);
        return task != null ? Ok(task) : NotFound($"Task with ID {id} not found.");
    }

    [HttpPost]
    [Authorize]
    public ActionResult Add([FromBody] TaskModel model)
    {
        setUser();
        if (_applicationUser is null){
            return BadRequest("Malformed Token");
        }

        var task = new TaskEntity
        {
            Title = model.Title,
            Status = model.Status
        };

        string result = _ts.Add(task,_applicationUser.Id);
        return Ok(result);
    }

    [HttpPut]
    [Authorize]
    public ActionResult Update([FromBody] TaskUpdate model)
    {
        setUser();
        if (_applicationUser is null){
            return BadRequest("Malformed Token");
        }

        string result = _ts.Update(model,_applicationUser.Id);
        return Ok(result);
    }

    [HttpPatch]
    [Authorize]
    public ActionResult Patch([FromBody] TaskPatch model)
    {
        setUser();
        if (_applicationUser is null){
            return BadRequest("Malformed Token");
        }

        string result = _ts.Patch(model,_applicationUser.Id);
        return Ok(result);
    }

    void setUser(){
        string? userId = User.FindFirst(nameof(ApplicationUser.Id))?.Value;
        string? firstName = User.FindFirst(nameof(ApplicationUser.FirstName))?.Value;
        string? lastName = User.FindFirst(nameof(ApplicationUser.LastName))?.Value;
        string? email = User.FindFirst(nameof(ApplicationUser.Email))?.Value;

        if (int.TryParse(userId, out int id) && firstName is not null && lastName is not null && email is not null)
        {
            _applicationUser = new(){
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }
    }
}