using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Entities;
using TodoApp.Interfaces;

namespace TodoApp.Services;

public class TodoListService(ITodoListRepository todoListRepository)
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;

    public async Task<IEnumerable<TodoListDto>> GetAllByUserIdAsync(int userId)
    {
        var lists = await _todoListRepository.GetAllByUserIdAsync(userId);
        return lists.Select(l => new TodoListDto { Id = l.Id, Name = l.Name });
    }

    public async Task<TodoListDto?> GetByIdAsync(int id)
    {
        var list = await _todoListRepository.GetByIdAsync(id);
        return list == null ? null : new TodoListDto { Id = list.Id, Name = list.Name };
    }

    public async Task<TodoListDto> AddAsync(TodoListDto dto, int userId)
    {
        if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentException("Name required");

        var list = new TodoList { Name = dto.Name, UserId = userId };
        await _todoListRepository.AddAsync(list);

        return new TodoListDto { Id = list.Id, Name = list.Name };
    }

    public async Task<TodoListDto> UpdateAsync(TodoListDto dto)
    {
        var list = await _todoListRepository.GetByIdAsync(dto.Id);
        if (list == null) throw new KeyNotFoundException("List not found");

        list.Name = dto.Name;
        await _todoListRepository.UpdateAsync(list);

        return new TodoListDto { Id = list.Id, Name = list.Name };
    }

    public async Task DeleteAsync(int id)
    {
        await _todoListRepository.DeleteAsync(id);
    }
}
