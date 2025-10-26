using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.DTOs;

namespace TodoApp.Interfaces
{
    public interface ITodoListService
    {
        Task<TodoListDto> AddAsync(TodoListDto dto, int userId);
        Task DeleteAsync(int id);
        Task<IEnumerable<TodoListDto>> GetAllByUserIdAsync(int userId);
        Task<TodoListDto?> GetByIdAsync(int id);
        Task<TodoListDto> UpdateAsync(TodoListDto dto);
    }
}