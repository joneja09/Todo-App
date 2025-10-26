namespace TodoApp.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public int TodoListId { get; set; }
    public TodoList? TodoList { get; set; }
}
