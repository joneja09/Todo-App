using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Interfaces;
using TodoApp.Persistence;
using TodoApp.Repositories;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("api", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TodoListService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<IdentityUser<int>>();

app.Run();
