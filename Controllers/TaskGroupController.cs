using ToDo_RestAPI.Data;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/taskgroups")]
[ApiController]
public class TaskGroupController(TodoDbContext db) : Controller
{
    // GET: api/taskgroups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskGroup>>> GetTaskGroups()
    {
        return await db.TaskGroups.ToListAsync();
    }

    // GET: api/taskgroups/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskGroup>> GetTaskGroupById(int id)
    {
        var taskGroup = await db.TaskGroups.FindAsync(id);

        return taskGroup == null ? NotFound() : taskGroup;
    }

    // POST: api/taskgroups
    [HttpPost]
    public async Task<ActionResult<TaskGroup>> CreateTaskGroup(TaskGroup taskGroup)
    {
        db.TaskGroups.Add(taskGroup);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskGroupById), new { id = taskGroup.Id }, taskGroup);
    }

    // PUT: api/taskgroups/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTaskGroup(int id, TaskGroup taskGroup)
    {
        if (id != taskGroup.Id)
        {
            return BadRequest();
        }

        db.Entry(taskGroup).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!db.TaskGroups.Any(e => e.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/taskgroups/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaskGroup(int id)
    {
        var taskGroup = await db.TaskGroups.FindAsync(id);
        if (taskGroup == null)
        {
            return NotFound();
        }

        db.TaskGroups.Remove(taskGroup);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
