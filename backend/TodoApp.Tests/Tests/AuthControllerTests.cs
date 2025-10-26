//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using TodoApp.Controllers;
//using TodoApp.DTOs;
//using TodoApp.Responses;
//using Xunit;

//namespace TodoApp.Tests.Tests;

//public class AuthControllerTests
//{
//    private readonly Mock<UserManager<IdentityUser<int>>> _mockUserManager;
//    private readonly Mock<SignInManager<IdentityUser<int>>> _mockSignInManager;
//    private readonly AuthController _controller;

//    public AuthControllerTests()
//    {
//        _mockUserManager = new Mock<UserManager<IdentityUser<int>>>(Mock.Of<IUserStore<IdentityUser<int>>>(), null!, null!, null!, null!, null!, null!, null!, null!);
//        _mockSignInManager = new Mock<SignInManager<IdentityUser<int>>>(_mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser<int>>>(), null!, null!, null!, null!);
//        _controller = new AuthController(_mockUserManager.Object, _mockSignInManager.Object);
//    }

//    [Fact]
//    public async Task Register_ThrowsIfUserExists()
//    {
//        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<IdentityUser<int>>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User exists" }));

//        var result = await _controller.Register(new UserDto { Username = "test", Password = "pass" });

//        Assert.False(result.Success);
//        Assert.Equal("User exists", result.Message);
//    }
//}
