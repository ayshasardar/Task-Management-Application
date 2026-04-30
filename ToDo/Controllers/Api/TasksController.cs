using Microsoft.AspNetCore.Mvc;
using ToDoDemo.Models;
using ToDoDemo.Services.Implementations;
using ToDoDemo.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IToDoService _toDoService;

    public TasksController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }

    [HttpPost("complete/{id}")]
    public IActionResult MarkComplete(int id)
    {
        var success = _toDoService.MarkComplete(id);

        if (!success)
            return NotFound();

        return Ok(new { success = true });
    }

    [HttpPost("predict-priority")]
    //I created this API endpoint to expose my ML prediction as a reusable service outside the
    //MVCflow, not just during the 'ADD Task' form submission 
    //It allows features like AJAX live prediction, mobile apps, or future frontends to call it independently.
    public IActionResult PredictPriority([FromBody] ToDo task)
    {
        if (task == null)
            return BadRequest();

        var result = _toDoService.PredictPriority(task);

        return Ok(new { predictedPriority = result });
    }
}