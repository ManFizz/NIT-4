using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_RestAPI.Data;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Controllers;

public class UserController(TodoDbContext db) : Controller
{
    // POST /user
    [Authorize]
    [HttpPost("/user")]
    public IActionResult AddUser([FromBody] User? user)
    {
        if (user == null || string.IsNullOrEmpty(user.UserName))
            return BadRequest("Invalid user data");

        db.Users.Add(user);
        db.SaveChanges();

        return Ok(user);
    }
}