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

    public async Task<IEnumerable<TaskItem>> GetAllByListIdAsync(int todoListId)
    {
        return await _context.Tasks.Where(t => t.TodoListId == todoListId).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

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
