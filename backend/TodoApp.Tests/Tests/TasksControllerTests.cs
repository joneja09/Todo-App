using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.DTOs;
using TodoApp.Responses;
using TodoApp.Services;
using Xunit;

namespace TodoApp.Tests.Tests;

public class TasksControllerTests
{
    private readonly Mock<TaskService> _mockService;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mockService = new Mock<TaskService>();
        _controller = new TasksController(_mockService.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
        _controller.ControllerContext = new ControllerContext { HttpContext = { User = user } };
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        _mockService.Setup(s => s.GetAllByListIdAsync(1)).ReturnsAsync(new List<TaskDto> { new() { Id = 1, Title = "Test", TodoListId = 1 } });

        var result = await _controller.GetAll(1);

        Assert.True(result.Success);
        Assert.Single(result.Data!);
    }
}
