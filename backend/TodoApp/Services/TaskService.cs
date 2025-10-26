using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Entities;
using TodoApp.Interfaces;

namespace TodoApp.Services;

public class TaskService(ITaskRepository taskRepository)
{
    private readonly ITaskRepository _taskRepository = taskRepository;

    public async Task<IEnumerable<TaskDto>> GetAllByListIdAsync(int todoListId)
    {
        var tasks = await _taskRepository.GetAllByListIdAsync(todoListId);
        return tasks.Select(t => new TaskDto { Id = t.Id, Title = t.Title, Description = t.Description, IsCompleted = t.IsCompleted, TodoListId = t.TodoListId });
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task == null ? null : new TaskDto { Id = task.Id, Title = task.Title, Description = task.Description, IsCompleted = task.IsCompleted, TodoListId = task.TodoListId };
    }

    public async Task AddAsync(TaskDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new ArgumentException("Title required");

        var task = new TaskItem { Title = dto.Title, Description = dto.Description, TodoListId = dto.TodoListId };
        await _taskRepository.AddAsync(task);
    }

    public async Task UpdateAsync(TaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(dto.Id);
        if (task == null) throw new KeyNotFoundException("Task not found");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.TodoListId = dto.TodoListId;
        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteAsync(int id)
    {
        await _taskRepository.DeleteAsync(id);
    }
}
