using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Entities;

namespace TodoApp.Interfaces;

/// <summary>
/// Defines a contract for managing and accessing to-do lists in a data store.
/// </summary>
/// <remarks>This interface provides methods for retrieving, adding, updating, and deleting to-do lists.
/// Implementations of this interface are expected to handle data persistence and retrieval for to-do lists associated
/// with specific users.</remarks>
public interface ITodoListRepository
{
    Task<IEnumerable<TodoList>> GetAllByUserIdAsync(int userId);
    Task<TodoList?> GetByIdAsync(int id);
    Task<TodoList?> GetByIdAndUserIdAsync(int id, int userId);
    Task AddAsync(TodoList list);
    Task UpdateAsync(TodoList list);
    Task DeleteAsync(int id);
}
