using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Entities;
using TodoApp.Interfaces;

namespace TodoApp.Services;

/// <summary>
/// Provides operations for managing to-do lists, including retrieving, adding, updating, and deleting lists.
/// </summary>
/// <remarks>This service acts as an intermediary between the application and the underlying repository,  ensuring
/// that to-do lists are managed in a consistent and reliable manner. It supports operations  such as retrieving all
/// lists for a specific user, retrieving a list by its ID, adding new lists,  updating existing lists, and deleting
/// lists.</remarks>
/// <param name="todoListRepository"></param>
public class TodoListService(ITodoListRepository todoListRepository) : ITodoListService
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;

    /// <summary>
    /// Retrieves all to-do lists for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose lists to retrieve.</param>
    /// <returns>A collection of <see cref="TodoListDto"/> objects.</returns>
    public async Task<IEnumerable<TodoListDto>> GetAllByUserIdAsync(int userId)
    {
        var lists = await _todoListRepository.GetAllByUserIdAsync(userId);
        return lists.Select(l => new TodoListDto { Id = l.Id, Name = l.Name });
    }

    /// <summary>
    /// Retrieves a to-do list by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The <see cref="TodoListDto"/> if found; otherwise, null.</returns>
    public async Task<TodoListDto?> GetByIdAsync(int id, int userId)
    {
        var list = await _todoListRepository.GetByIdAndUserIdAsync(id, userId);
        return list == null ? null : new TodoListDto { Id = list.Id, Name = list.Name };
    }

    /// <summary>
    /// Adds a new to-do list for a specific user.
    /// </summary>
    /// <param name="dto">The to-do list data transfer object.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The created <see cref="TodoListDto"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the name is missing.</exception>
    public async Task<TodoListDto> AddAsync(TodoListDto dto, int userId)
    {
        if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentException("Name required");

        var list = new TodoList { Name = dto.Name, UserId = userId };
        await _todoListRepository.AddAsync(list);

        return new TodoListDto { Id = list.Id, Name = list.Name };
    }

    /// <summary>
    /// Updates an existing to-do list for a specific user.
    /// </summary>
    /// <param name="dto">The to-do list data transfer object.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The updated <see cref="TodoListDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the list is not found.</exception>
    public async Task<TodoListDto> UpdateAsync(TodoListDto dto, int userId)
    {
        var list = await _todoListRepository.GetByIdAndUserIdAsync(dto.Id, userId);
        
        if (list == null) throw new KeyNotFoundException("List not found");

        list.Name = dto.Name;
        await _todoListRepository.UpdateAsync(list);

        return new TodoListDto { Id = list.Id, Name = list.Name };
    }

    /// <summary>
    /// Deletes a to-do list for a specific user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the list is not found.</exception>
    public async Task DeleteAsync(int id, int userId)
    {
        var list = await _todoListRepository.GetByIdAndUserIdAsync(id, userId);
        
        if (list == null) throw new KeyNotFoundException("List not found");
        
        await _todoListRepository.DeleteAsync(id);
    }
}
