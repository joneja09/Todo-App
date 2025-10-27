using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Extensions;
using TodoApp.Interfaces;
using TodoApp.Responses;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("api")]
public class TodoListsController(ITodoListService todoListService) : ControllerBase
{
    private readonly ITodoListService _todoListService = todoListService;

    /// <summary>
    /// Retrieves all to-do lists associated with the currently authenticated user.
    /// </summary>
    /// <remarks>This method fetches all to-do lists for the user identified by the authentication token.  The
    /// user must be authenticated to call this method.</remarks>
    /// <returns>An <see cref="ApiResponse{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="TodoListDto"/> 
    /// representing the to-do lists for the current user.</returns>
    /// <summary>
    /// Retrieves all to-do lists associated with the currently authenticated user.
    /// </summary>
    /// <returns>An API response containing the user's to-do lists.</returns>
    [HttpGet]
    [Route("")]
    public async Task<ApiResponse<IEnumerable<TodoListDto>>> GetAll()
    {
        var userId = User.GetUserId();
        var lists = await _todoListService.GetAllByUserIdAsync(userId);
        return ApiResponse<IEnumerable<TodoListDto>>.Ok(lists);
    }

    /// <summary>
    /// Retrieves a to-do list by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the to-do list to retrieve.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the to-do list data if found; otherwise, an error response indicating
    /// that the to-do list was not found.</returns>
    /// <summary>
    /// Retrieves a specific to-do list by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>An API response containing the to-do list if found, or an error message.</returns>
    [HttpGet("{id}")]
    public async Task<ApiResponse<TodoListDto>> Get(int id)
    {
        var userId = User.GetUserId();
        var list = await _todoListService.GetByIdAsync(id, userId);
        return list == null ? ApiResponse<TodoListDto>.Error("Not found") : ApiResponse<TodoListDto>.Ok(list);
    }

    /// <summary>
    /// Creates a new to-do list for the authenticated user.
    /// </summary>
    /// <remarks>The authenticated user's ID is extracted from the claims to associate the to-do list with the
    /// correct user.</remarks>
    /// <param name="dto">The data transfer object containing the details of the to-do list to be created.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the created to-do list object if the operation is successful; 
    /// otherwise, an error response with the exception message.</returns>
    /// <summary>
    /// Creates a new to-do list for the authenticated user.
    /// </summary>
    /// <param name="dto">The to-do list data transfer object.</param>
    /// <returns>An API response containing the created to-do list or an error message.</returns>
    [HttpPost]
    [Route("")]
    public async Task<ApiResponse<TodoListDto>> Post([FromBody] TodoListDto dto)
    {
        try
        {
            var userId = User.GetUserId();
            var list = await _todoListService.AddAsync(dto, userId);
            return ApiResponse<TodoListDto>.Ok(list);
        }
        catch (Exception ex)
        {
            return ApiResponse<TodoListDto>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing to-do list for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="dto">The updated to-do list data transfer object.</param>
    /// <returns>An API response containing the updated to-do list or an error message.</returns>
    [HttpPut("{id}")]
    public async Task<ApiResponse<TodoListDto>> Put(int id, [FromBody] TodoListDto dto)
    {
        try
        {
            var userId = User.GetUserId();
            dto.Id = id;
            var list = await _todoListService.UpdateAsync(dto, userId);
            return ApiResponse<TodoListDto>.Ok(list);
        }
        catch (Exception ex)
        {
            return ApiResponse<TodoListDto>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a to-do list for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the to-do list to delete.</param>
    /// <returns>An API response indicating success or an error message.</returns>
    [HttpDelete("{id}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        try
        {
            var userId = User.GetUserId();
            await _todoListService.DeleteAsync(id, userId);
            return ApiResponse<object>.Ok(null!);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.Error(ex.Message);
        }
    }
}
