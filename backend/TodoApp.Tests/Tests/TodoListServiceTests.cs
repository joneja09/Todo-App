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
}
