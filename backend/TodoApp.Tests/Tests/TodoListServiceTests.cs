using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.DTOs;
using TodoApp.Entities;
using TodoApp.Interfaces;
using TodoApp.Services;
using Xunit;

namespace TodoApp.Tests.Tests;

public class TodoListServiceTests
{
    private readonly Mock<ITodoListRepository> _mockRepo;
    private readonly TodoListService _service;

    public TodoListServiceTests()
    {
        _mockRepo = new Mock<ITodoListRepository>();
        _service = new TodoListService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_ReturnsLists()
    {
        var lists = new List<TodoList> { new TodoList { Id = 1, Name = "Test List", UserId = 1 } };
        _mockRepo.Setup(r => r.GetAllByUserIdAsync(1)).ReturnsAsync(lists);

        var result = await _service.GetAllByUserIdAsync(1);

        Assert.Single(result);
        Assert.Equal("Test List", result.First().Name);
    }

    [Fact]
    public async Task AddAsync_ThrowsIfNoName()
    {
        var dto = new TodoListDto { Name = "" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto, 1));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsList_WhenFound()
    {
        var list = new TodoList { Id = 2, Name = "Sample", UserId = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(list);

        var result = await _service.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("Sample", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TodoList?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsListSuccessfully()
    {
        var dto = new TodoListDto { Name = "New List" };
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<TodoList>())).Returns(Task.CompletedTask);

        var result = await _service.AddAsync(dto, 1);

        Assert.Equal("New List", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesListSuccessfully()
    {
        var dto = new TodoListDto { Id = 1, Name = "Updated" };
        var list = new TodoList { Id = 1, Name = "Old", UserId = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(list);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TodoList>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(dto);

        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_DeletesListSuccessfully()
    {
        _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
