using Microsoft.AspNetCore.Mvc;
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
}