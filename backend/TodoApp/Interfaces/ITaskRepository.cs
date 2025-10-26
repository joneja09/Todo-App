using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Entities;

namespace TodoApp.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllByListIdAsync(int todoListId);
    Task<TaskItem?> GetByIdAsync(int id);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(int id);
}
