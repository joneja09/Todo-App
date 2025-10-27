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

/// <summary>
/// Provides endpoints for managing tasks within a to-do list.
/// </summary>
/// <remarks>This controller exposes CRUD operations for tasks, including retrieving tasks by their ID or list ID,
/// creating new tasks, updating existing tasks, and deleting tasks. All endpoints require authorization  with the "api"
/// policy.</remarks>
/// <param name="taskService"></param>
[ApiController]
[Route("api/[controller]")]
[Authorize("api")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    /// <summary>
    /// Retrieves all tasks for a specific to-do list and user.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>An API response containing the tasks for the specified list.</returns>
    [HttpGet("list/{todoListId}")]
    public async Task<ApiResponse<IEnumerable<TaskDto>>> GetAll(int todoListId)
    {
        var userId = User.GetUserId();
        var tasks = await _taskService.GetAllByListIdAsync(todoListId, userId);
        return ApiResponse<IEnumerable<TaskDto>>.Ok(tasks);
    }

    /// <summary>
    /// Retrieves a specific task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>An API response containing the task if found, or an error message.</returns>
    [HttpGet("{id}")]
    public async Task<ApiResponse<TaskDto>> Get(int id)
    {
        var userId = User.GetUserId();
        var task = await _taskService.GetByIdAsync(id, userId);
        return task == null ? ApiResponse<TaskDto>.Error("Not found") : ApiResponse<TaskDto>.Ok(task);
    }

    /// <summary>
    /// Creates a new task for the authenticated user.
    /// </summary>
    /// <param name="dto">The task data transfer object.</param>
    /// <returns>An API response containing the created task or an error message.</returns>
    [HttpPost]
    public async Task<ApiResponse<TaskDto>> Post([FromBody] TaskDto dto)
    {
        try
        {
            var userId = User.GetUserId();
            var task = await _taskService.AddAsync(dto, userId);
            return ApiResponse<TaskDto>.Ok(task);
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing task for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="dto">The updated task data transfer object.</param>
    /// <returns>An API response containing the updated task or an error message.</returns>
    [HttpPut("{id}")]
    public async Task<ApiResponse<TaskDto>> Put(int id, [FromBody] TaskDto dto)
    {
        try
        {
            var userId = User.GetUserId();
            dto.Id = id;
            var task = await _taskService.UpdateAsync(dto, userId);
            return ApiResponse<TaskDto>.Ok(task);
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto>.Error(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a task for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <returns>An API response indicating success or an error message.</returns>
    [HttpDelete("{id}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        try
        {
            var userId = User.GetUserId();
            await _taskService.DeleteAsync(id, userId);
            return ApiResponse<object>.Ok(null!);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.Error(ex.Message);
        }
    }
}
