using ToDo_RestAPI.Data;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/tasklabels")]
[ApiController]
public class TaskLabelController(TodoDbContext db) : Controller
{
    // GET: api/tasklabels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskLabel>>> GetTaskLabels()
    {
        return await db.TaskLabels.ToListAsync();
    }

    // GET: api/tasklabels/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskLabel>> GetTaskLabelById(int id)
    {
        var taskLabel = await db.TaskLabels.FindAsync(id);

        if (taskLabel == null)
        {
            return NotFound();
        }

        return taskLabel;
    }

    // POST: api/tasklabels
    [HttpPost]
    public async Task<ActionResult<TaskLabel>> CreateTaskLabel(TaskLabel taskLabel)
    {
        db.TaskLabels.Add(taskLabel);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTaskLabelById), new { id = taskLabel.Id }, taskLabel);
    }

    // PUT: api/tasklabels/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTaskLabel(int id, TaskLabel taskLabel)
    {
        if (id != taskLabel.Id)
        {
            return BadRequest();
        }

        db.Entry(taskLabel).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!db.TaskLabels.Any(e => e.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/tasklabels/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaskLabel(int id)
    {
        var taskLabel = await db.TaskLabels.FindAsync(id);
        if (taskLabel == null)
        {
            return NotFound();
        }

        db.TaskLabels.Remove(taskLabel);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
