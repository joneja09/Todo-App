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

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepo;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _service = new TaskService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllByListIdAsync_ReturnsTasks()
    {
        var tasks = new List<TaskItem> { new TaskItem { Id = 1, Title = "Test", TodoListId = 1 } };
        _mockRepo.Setup(r => r.GetAllByListIdAsync(1)).ReturnsAsync(tasks);

        var result = await _service.GetAllByListIdAsync(1);

        Assert.Single(result);
        Assert.Equal("Test", result.First().Title);
    }

    [Fact]
    public async Task AddAsync_ThrowsIfNoTitle()
    {
        var dto = new TaskDto { Title = "", TodoListId = 1 };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddAsync(dto));
    }
}
