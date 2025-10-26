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

    public async Task<IEnumerable<TodoList>> GetAllByUserIdAsync(int userId)
    {
        return await _context.TodoLists.Where(l => l.UserId == userId).ToListAsync();
    }

    public async Task<TodoList?> GetByIdAsync(int id)
    {
        return await _context.TodoLists.FindAsync(id);
    }

    public async Task AddAsync(TodoList list)
    {
        _context.TodoLists.Add(list);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoList list)
    {
        _context.TodoLists.Update(list);
        await _context.SaveChangesAsync();
    }

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
