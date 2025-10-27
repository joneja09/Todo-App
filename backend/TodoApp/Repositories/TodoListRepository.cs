using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Entities;
using TodoApp.Interfaces;
using TodoApp.Persistence;

namespace TodoApp.Repositories;

/// <summary>
/// Provides methods for managing and accessing to-do lists in the data store.
/// </summary>
/// <remarks>This repository is responsible for performing CRUD (Create, Read, Update, Delete) operations  on <see
/// cref="TodoList"/> entities. It interacts with the underlying database context to  retrieve, add, update, and delete
/// to-do lists. The repository is designed to work with  user-specific to-do lists, allowing operations to be scoped to
/// a specific user where applicable.</remarks>
/// <param name="context"></param>
public class TodoListRepository(AppDbContext context) : ITodoListRepository
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Retrieves all to-do lists for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of <see cref="TodoList"/> entities.</returns>
    public async Task<IEnumerable<TodoList>> GetAllByUserIdAsync(int userId)
    {
        return await _context.TodoLists.Where(l => l.UserId == userId).ToListAsync();
    }

    /// <summary>
    /// Retrieves a to-do list by its ID.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>The <see cref="TodoList"/> if found; otherwise, null.</returns>
    public async Task<TodoList?> GetByIdAsync(int id)
    {
        return await _context.TodoLists.FindAsync(id);
    }

    /// <summary>
    /// Adds a new to-do list to the data store.
    /// </summary>
    /// <param name="list">The to-do list entity to add.</param>
    public async Task AddAsync(TodoList list)
    {
        _context.TodoLists.Add(list);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing to-do list in the data store.
    /// </summary>
    /// <param name="list">The to-do list entity to update.</param>
    public async Task UpdateAsync(TodoList list)
    {
        _context.TodoLists.Update(list);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a to-do list by its ID and user ID.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The <see cref="TodoList"/> if found; otherwise, null.</returns>
    public async Task<TodoList?> GetByIdAndUserIdAsync(int id, int userId)
    {
        return await _context.TodoLists.FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
    }

    /// <summary>
    /// Deletes a to-do list by its ID.
    /// </summary>
    /// <param name="id">The ID of the to-do list to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var list = await GetByIdAsync(id);
        if (list != null)
        {
            _context.TodoLists.Remove(list);
            await _context.SaveChangesAsync();
        }
    }
}
