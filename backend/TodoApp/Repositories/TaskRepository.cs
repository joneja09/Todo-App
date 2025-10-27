using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Entities;
using TodoApp.Interfaces;
using TodoApp.Persistence;

namespace TodoApp.Repositories;

/// <summary>
/// Provides methods for managing and accessing task items in the data store.
/// </summary>
/// <remarks>This repository is responsible for performing CRUD operations on <see cref="TaskItem"/> entities. It
/// interacts with the underlying database context to retrieve, add, update, and delete tasks.</remarks>
/// <param name="context"></param>
public class TaskRepository(AppDbContext context) : ITaskRepository
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Retrieves all tasks for a specific to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>A collection of <see cref="TaskItem"/> entities.</returns>
    public async Task<IEnumerable<TaskItem>> GetAllByListIdAsync(int todoListId)
    {
        return await _context.Tasks.Where(t => t.TodoListId == todoListId).ToListAsync();
    }

    /// <summary>
    /// Retrieves a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>The <see cref="TaskItem"/> if found; otherwise, null.</returns>
    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    /// <summary>
    /// Adds a new task to the data store.
    /// </summary>
    /// <param name="task">The task entity to add.</param>
    public async Task AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing task in the data store.
    /// </summary>
    /// <param name="task">The task entity to update.</param>
    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all tasks for a specific to-do list and user.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of <see cref="TaskItem"/> entities.</returns>
    public async Task<IEnumerable<TaskItem>> GetAllByListIdAndUserIdAsync(int todoListId, int userId)
    {
        return await _context.Tasks
            .Include(t => t.TodoList)
            .Where(t => t.TodoListId == todoListId && t.TodoList!.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a task by its ID and user ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The <see cref="TaskItem"/> if found; otherwise, null.</returns>
    public async Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId)
    {
        return await _context.Tasks
            .Include(t => t.TodoList)
            .FirstOrDefaultAsync(t => t.Id == id && t.TodoList!.UserId == userId);
    }

    /// <summary>
    /// Deletes a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var task = await GetByIdAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
