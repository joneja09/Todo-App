using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Entities;

namespace TodoApp.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>(options)
{
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TodoList> TodoLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TodoList>().HasOne(l => l.User).WithMany().HasForeignKey(l => l.UserId);
        modelBuilder.Entity<TaskItem>().HasOne(t => t.TodoList).WithMany(l => l.Tasks).HasForeignKey(t => t.TodoListId);
    }
}
