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

    [Fact]
    public async Task GetByIdAsync_ReturnsTask_WhenFound()
    {
        var task = new TaskItem { Id = 2, Title = "Sample", TodoListId = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(task);

        var result = await _service.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("Sample", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TaskItem?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTaskSuccessfully()
    {
        var dto = new TaskDto { Id = 1, Title = "Updated", TodoListId = 1 };
        var task = new TaskItem { Id = 1, Title = "Old", TodoListId = 1 };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(dto);

        Assert.Equal("Updated", result.Title);
    }

    [Fact]
    public async Task DeleteAsync_DeletesTaskSuccessfully()
    {
        _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}
