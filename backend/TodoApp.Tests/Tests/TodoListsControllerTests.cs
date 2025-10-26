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

public class TodoListsControllerTests
{
    private readonly Mock<ITodoListService> _mockService;
    private readonly TodoListsController _controller;

    public TodoListsControllerTests()
    {
        _mockService = new Mock<ITodoListService>();
        _controller = new TodoListsController(_mockService.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
        var httpContext = new DefaultHttpContext { User = user };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        _mockService.Setup(s => s.GetAllByUserIdAsync(1)).ReturnsAsync(new List<TodoListDto> { new TodoListDto { Id = 1, Name = "Test List" } });

        var result = await _controller.GetAll();

        Assert.True(result.Success);
        Assert.Single(result.Data!);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenListExists()
    {
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new TodoListDto { Id = 1, Name = "Test List" });

        var result = await _controller.Get(1);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(1, result.Data!.Id);
    }

    [Fact]
    public async Task Get_ReturnsError_WhenListDoesNotExist()
    {
        _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((TodoListDto?)null);

        var result = await _controller.Get(99);

        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.NotNull(result.Message);
    }

    [Fact]
    public async Task Post_ReturnsOk_WhenListCreated()
    {
        var dto = new TodoListDto { Name = "New List" };
        _mockService.Setup(s => s.AddAsync(dto, 1)).ReturnsAsync(new TodoListDto { Id = 2, Name = "New List" });

        var result = await _controller.Post(dto);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("New List", ((TodoListDto)result.Data!).Name);
    }

    [Fact]
    public async Task Put_ReturnsOk_WhenListUpdated()
    {
        var dto = new TodoListDto { Id = 1, Name = "Updated List" };
        _mockService.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(new TodoListDto { Id = 1, Name = "Updated List" });

        var result = await _controller.Put(1, dto);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Updated List", result.Data!.Name);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenListDeleted()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.True(result.Success);
        Assert.Null(result.Data);
    }
}
