using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.DTOs;

namespace TodoApp.Interfaces
{
    /// <summary>
    /// Defines a contract for managing tasks, including operations to add, update, delete, and retrieve tasks.
    /// </summary>
    /// <remarks>This interface provides asynchronous methods for interacting with tasks, allowing for CRUD
    /// operations and retrieval of tasks by specific criteria. Implementations of this interface are expected to handle
    /// the underlying data storage and retrieval mechanisms.</remarks>
    public interface ITaskService
    {
        Task<TaskDto> AddAsync(TaskDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<TaskDto>> GetAllByListIdAsync(int todoListId);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskDto> UpdateAsync(TaskDto dto);
    }
}