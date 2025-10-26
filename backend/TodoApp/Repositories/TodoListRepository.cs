using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Entities;
using TodoApp.Interfaces;
using TodoApp.Persistence;

namespace TodoApp.Repositories;

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
