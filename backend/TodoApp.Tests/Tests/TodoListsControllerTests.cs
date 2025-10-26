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

public class TodoListsControllerTests
{
    private readonly Mock<TodoListService> _mockService;
    private readonly TodoListsController _controller;

    public TodoListsControllerTests()
    {
        _mockService = new Mock<TodoListService>();
        _controller = new TodoListsController(_mockService.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
        _controller.ControllerContext = new ControllerContext { HttpContext = { User = user } };
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        _mockService.Setup(s => s.GetAllByUserIdAsync(1)).ReturnsAsync(new List<TodoListDto> { new TodoListDto { Id = 1, Name = "Test List" } });

        var result = await _controller.GetAll();

        Assert.True(result.Success);
        Assert.Single(result.Data!);
    }
}
