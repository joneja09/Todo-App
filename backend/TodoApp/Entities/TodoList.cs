using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TodoApp.Entities;

public class TodoList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public IdentityUser<int>? User { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
