using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Entities;

namespace TodoApp.Interfaces;

public interface ITodoListRepository
{
    Task<IEnumerable<TodoList>> GetAllByUserIdAsync(int userId);
    Task<TodoList?> GetByIdAsync(int id);
    Task AddAsync(TodoList list);
    Task UpdateAsync(TodoList list);
    Task DeleteAsync(int id);
}
