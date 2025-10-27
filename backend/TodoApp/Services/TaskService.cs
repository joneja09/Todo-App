using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Entities;
using TodoApp.Interfaces;

namespace TodoApp.Services;

/// <summary>
/// Provides operations for managing tasks, including retrieving, adding, updating, and deleting tasks.
/// </summary>
/// <remarks>This service acts as an abstraction layer for task-related operations, interacting with the
/// underlying data repository to perform CRUD operations. It ensures that tasks are properly validated and mapped
/// between the data model and the DTO (Data Transfer Object) used by the application.</remarks>
/// <param name="taskRepository"></param>
/// <param name="todoListRepository"></param>
public class TaskService(ITaskRepository taskRepository, ITodoListRepository todoListRepository) : ITaskService
{
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly ITodoListRepository _todoListRepository = todoListRepository;

    /// <summary>
    /// Retrieves all tasks for a specific to-do list and user.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of <see cref="TaskDto"/> objects.</returns>
    public async Task<IEnumerable<TaskDto>> GetAllByListIdAsync(int todoListId, int userId)
    {
        var tasks = await _taskRepository.GetAllByListIdAndUserIdAsync(todoListId, userId);
        return tasks.Select(t => new TaskDto { Id = t.Id, Title = t.Title, Description = t.Description, IsCompleted = t.IsCompleted, TodoListId = t.TodoListId });
    }

    /// <summary>
    /// Retrieves a task by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The <see cref="TaskDto"/> if found; otherwise, null.</returns>
    public async Task<TaskDto?> GetByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(id, userId);
        return task == null ? null : new TaskDto { Id = task.Id, Title = task.Title, Description = task.Description, IsCompleted = task.IsCompleted, TodoListId = task.TodoListId };
    }

    /// <summary>
    /// Adds a new task to a specific to-do list for a user.
    /// </summary>
    /// <param name="dto">The task data transfer object.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The created <see cref="TaskDto"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the title is missing.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the to-do list is not found or access is denied.</exception>
    public async Task<TaskDto> AddAsync(TaskDto dto, int userId)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new ArgumentException("Title required");

        // Validate that the user owns the TodoList
        var todoList = await _todoListRepository.GetByIdAndUserIdAsync(dto.TodoListId, userId);
        if (todoList == null) throw new KeyNotFoundException("TodoList not found or access denied");

        var task = new TaskItem { Title = dto.Title, Description = dto.Description, TodoListId = dto.TodoListId };
        await _taskRepository.AddAsync(task);

        return new TaskDto { Id = task.Id, Title = task.Title, Description = task.Description, IsCompleted = task.IsCompleted, TodoListId = task.TodoListId };
    }

    /// <summary>
    /// Updates an existing task for a specific user.
    /// </summary>
    /// <param name="dto">The task data transfer object.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The updated <see cref="TaskDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
    public async Task<TaskDto> UpdateAsync(TaskDto dto, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(dto.Id, userId);
        if (task == null) throw new KeyNotFoundException("Task not found");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.TodoListId = dto.TodoListId;
        await _taskRepository.UpdateAsync(task);

        return new TaskDto { Id = task.Id, Title = task.Title, Description = task.Description, IsCompleted = task.IsCompleted, TodoListId = task.TodoListId };
    }

    /// <summary>
    /// Deletes a task for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
    public async Task DeleteAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAndUserIdAsync(id, userId);
        if (task == null) throw new KeyNotFoundException("Task not found");

        await _taskRepository.DeleteAsync(id);
    }
}
