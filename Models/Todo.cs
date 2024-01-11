namespace ToDo_RestAPI.Models;

public class Todo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Task { get; set; }
}