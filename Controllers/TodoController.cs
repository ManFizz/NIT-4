using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_RestAPI.Data;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Controllers;

public class TodoController(TodoDbContext db) : Controller
{
    // GET /todo
    [Authorize]
    [HttpGet("/todo")]
    public IActionResult GetTodos([FromQuery] int userId)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound($"User with ID {userId} not found");

        var userTodos = db.Todos.Where(t => t.UserId == userId).ToList();
        
        return Ok(userTodos);
    }

    // POST /todo
    [HttpPost("/todo")]
    public IActionResult AddTodo([FromBody] Todo todo)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == todo.UserId);
        if (user == null)
            return NotFound($"User with ID {todo.UserId} not found");

        db.Todos.Add(todo);
        db.SaveChanges();

        return Ok(todo);
    }

    // DELETE /todo/{id}
    [HttpDelete("/todo/{id:int}")]
    public IActionResult DeleteTodo(int id)
    {
        var todo = db.Todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound($"Todo with ID {id} not found");

        db.Todos.Remove(todo);
        db.SaveChanges();
        
        return Ok(todo);
    }

    // PUT /todo/{id}
    [HttpPut("/todo/{id:int}")]
    public IActionResult UpdateTodo(int id, [FromBody] Todo updatedTodo)
    {
        var todo = db.Todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound($"Todo with ID {id} not found");

        todo.Task = updatedTodo.Task;
        db.SaveChanges();
        
        return Ok(todo);
    }
}
