using ToDo_RestAPI.Data;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/useractions")]
[ApiController]
public class UserActionController(TodoDbContext db) : Controller
{
    // GET: api/useractions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserAction>>> GetUserActions()
    {
        return await db.UserActions.ToListAsync();
    }

    // GET: api/useractions/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserAction>> GetUserActionById(int id)
    {
        var userAction = await db.UserActions.FindAsync(id);

        if (userAction == null)
        {
            return NotFound();
        }

        return userAction;
    }

    // POST: api/useractions
    [HttpPost]
    public async Task<ActionResult<UserAction>> CreateUserAction(UserAction userAction)
    {
        db.UserActions.Add(userAction);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserActionById), new { id = userAction.Id }, userAction);
    }

    // PUT: api/useractions/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUserAction(int id, UserAction userAction)
    {
        if (id != userAction.Id)
        {
            return BadRequest();
        }

        db.Entry(userAction).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!db.UserActions.Any(e => e.Id == id))
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

    // DELETE: api/useractions/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserAction(int id)
    {
        var userAction = await db.UserActions.FindAsync(id);
        if (userAction == null)
        {
            return NotFound();
        }

        db.UserActions.Remove(userAction);
        await db.SaveChangesAsync();

        return NoContent();
    }
}