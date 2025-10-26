using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.DTOs;
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

    [HttpGet("list/{todoListId}")]
    public async Task<ApiResponse<IEnumerable<TaskDto>>> GetAll(int todoListId)
    {
        var tasks = await _taskService.GetAllByListIdAsync(todoListId);
        return ApiResponse<IEnumerable<TaskDto>>.Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<TaskDto>> Get(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        return task == null ? ApiResponse<TaskDto>.Error("Not found") : ApiResponse<TaskDto>.Ok(task);
    }

    [HttpPost]
    public async Task<ApiResponse<TaskDto?>> Post([FromBody] TaskDto dto)
    {
        try
        {
            var task = await _taskService.AddAsync(dto);
            return ApiResponse<TaskDto?>.Ok(task);
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto?>.Error(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<TaskDto?>> Put(int id, [FromBody] TaskDto dto)
    {
        try
        {
            dto.Id = id;
            var task = await _taskService.UpdateAsync(dto);
            return ApiResponse<TaskDto?>.Ok(task);
        }
        catch (Exception ex)
        {
            return ApiResponse<TaskDto?>.Error(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<object>> Delete(int id)
    {
        try
        {
            await _taskService.DeleteAsync(id);
            return ApiResponse<object>.Ok(null!);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.Error(ex.Message);
        }
    }
}
