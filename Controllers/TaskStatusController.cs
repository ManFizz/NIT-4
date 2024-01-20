using ToDo_RestAPI.Data;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/taskstatuses")]
[ApiController]
public class TaskStatusController(TodoDbContext db) : Controller
{
    // GET: api/taskstatuses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.TaskStatus>>> GetTaskStatuses()
    {
        return await db.TaskStatuses.ToListAsync();
    }

    // GET: api/taskstatuses/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.TaskStatus>> GetTaskStatusById(int id)
    {
        var taskStatus = await db.TaskStatuses.FindAsync(id);

        if (taskStatus == null)
        {
            return NotFound();
        }

        return taskStatus;
    }

    // POST: api/taskstatuses
    [HttpPost]
    public async Task<ActionResult<Models.TaskStatus>> CreateTaskStatus(Models.TaskStatus taskStatus)
    {
        db.TaskStatuses.Add(taskStatus);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskStatusById), new { id = taskStatus.Id }, taskStatus);
    }

    // PUT: api/taskstatuses/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTaskStatus(int id, Models.TaskStatus taskStatus)
    {
        if (id != taskStatus.Id)
        {
            return BadRequest();
        }

        db.Entry(taskStatus).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!db.TaskStatuses.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/taskstatuses/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaskStatus(int id)
    {
        var taskStatus = await db.TaskStatuses.FindAsync(id);
        if (taskStatus == null)
        {
            return NotFound();
        }

        db.TaskStatuses.Remove(taskStatus);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
