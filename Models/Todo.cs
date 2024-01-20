namespace ToDo_RestAPI.Models;

public class Todo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Task { get; set; }

    public int TaskStatusId { get; set; }
    public TaskStatus TaskStatus { get; set; }

    public int TaskLabelId { get; set; }
    public TaskLabel TaskLabel { get; set; }

    public int TaskGroupId { get; set; }
    public TaskGroup TaskGroup { get; set; }

    public User User { get; set; }

    // Дополнительные свойства и связи, если необходимо
}