namespace ToDo_RestAPI.Models;

using Microsoft.EntityFrameworkCore;

public sealed class TodoDbContext : DbContext
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
}
