using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using front.Models;
using apiExo.bll.services;
using apiExo.bll.entity;

namespace front.Controllers;

public class HomeController(ITaskService taskService,ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<TaskEntity>>> All()
    {
        var tasks = await taskService.GetAllAsync();
        return View(tasks);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
