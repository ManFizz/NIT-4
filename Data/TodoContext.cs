using System.Diagnostics.CodeAnalysis;
using ToDo_RestAPI.Models;

namespace ToDo_RestAPI.Data;

using Microsoft.EntityFrameworkCore;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
public class TodoDbContext : DbContext
{
    
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=app.db");
    }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Todo> Todos { get; set; }
    public DbSet<TaskLabel> TaskLabels { get; set; }
    public DbSet<Models.TaskStatus> TaskStatuses { get; set; }
    public DbSet<TaskGroup> TaskGroups { get; set; }
    public DbSet<UserAction> UserActions { get; set; }
}
