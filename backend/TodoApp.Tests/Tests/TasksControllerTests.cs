using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.DTOs;
using TodoApp.Interfaces;
using Xunit;

namespace TodoApp.Tests.Tests;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _mockService;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mockService = new Mock<ITaskService>();
        _controller = new TasksController(_mockService.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
        var httpContext = new DefaultHttpContext { User = user };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
    _mockService.Setup(s => s.GetAllByListIdAsync(1, 1)).ReturnsAsync(new List<TaskDto> { new() { Id = 1, Title = "Test", TodoListId = 1 } });

    var result = await _controller.GetAll(1);

    Assert.True(result.Success);
    Assert.Single(result.Data!);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenTaskExists()
    {
    _mockService.Setup(s => s.GetByIdAsync(1, 1)).ReturnsAsync(new TaskDto { Id = 1, Title = "Test", TodoListId = 1 });

    var result = await _controller.Get(1);

    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    Assert.Equal(1, result.Data!.Id);
    }

    [Fact]
    public async Task Get_ReturnsError_WhenTaskDoesNotExist()
    {
    _mockService.Setup(s => s.GetByIdAsync(99, 1)).ReturnsAsync((TaskDto?)null);

    var result = await _controller.Get(99);

    Assert.False(result.Success);
    Assert.Null(result.Data);
    Assert.NotNull(result.Message);
    }

    [Fact]
    public async Task Post_ReturnsOk_WhenTaskCreated()
    {
    var dto = new TaskDto { Title = "New Task", TodoListId = 1 };
    _mockService.Setup(s => s.AddAsync(dto, 1)).ReturnsAsync(new TaskDto { Id = 2, Title = "New Task", TodoListId = 1 });

    var result = await _controller.Post(dto);

    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    Assert.Equal("New Task", ((TaskDto)result.Data!).Title);
    }

    [Fact]
    public async Task Put_ReturnsOk_WhenTaskUpdated()
    {
    var dto = new TaskDto { Id = 1, Title = "Updated Task", TodoListId = 1 };
    _mockService.Setup(s => s.UpdateAsync(dto, 1)).ReturnsAsync(new TaskDto { Id = 1, Title = "Updated Task", TodoListId = 1 });

    var result = await _controller.Put(1, dto);

    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    Assert.Equal("Updated Task", result.Data!.Title);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenTaskDeleted()
    {
    _mockService.Setup(s => s.DeleteAsync(1, 1)).Returns(Task.CompletedTask);

    var result = await _controller.Delete(1);

    Assert.True(result.Success);
    Assert.Null(result.Data);
    }
}
