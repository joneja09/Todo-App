using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Responses;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("api")]
public class TasksController(TaskService taskService) : ControllerBase
{
    private readonly TaskService _taskService = taskService;

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
    public async Task<ApiResponse<object>> Post([FromBody] TaskDto dto)
    {
        try
        {
            await _taskService.AddAsync(dto);
            return ApiResponse<object>.Ok(null!);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.Error(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<object>> Put(int id, [FromBody] TaskDto dto)
    {
        try
        {
            dto.Id = id;
            await _taskService.UpdateAsync(dto);
            return ApiResponse<object>.Ok(null!);
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.Error(ex.Message);
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
