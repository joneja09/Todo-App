using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Entities;

namespace TodoApp.Interfaces;

/// <summary>
/// Defines a contract for managing task items in a to-do list repository.
/// </summary>
/// <remarks>This interface provides methods for retrieving, adding, updating, and deleting task items.
/// Implementations of this interface are expected to handle data persistence and retrieval for task items associated
/// with specific to-do lists.</remarks>
public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllByListIdAsync(int todoListId);
    Task<IEnumerable<TaskItem>> GetAllByListIdAndUserIdAsync(int todoListId, int userId);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(int id);
}
