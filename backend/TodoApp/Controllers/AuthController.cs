//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using TodoApp.DTOs;
//using TodoApp.Responses;

//namespace TodoApp.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class AuthController : ControllerBase
//{
//    private readonly UserManager<IdentityUser<int>> _userManager;
//    private readonly SignInManager<IdentityUser<int>> _signInManager;
//    private readonly string _jwtSecret = "your-secret-key-here-min-32-chars";

//    public AuthController(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager)
//    {
//        _userManager = userManager;
//        _signInManager = signInManager;
//    }

//    [HttpPost("register")]
//    public async Task<ApiResponse<string>> Register([FromBody] UserDto dto)
//    {
//        try
//        {
//            var user = new IdentityUser<int> { UserName = dto.Username };
//            var result = await _userManager.CreateAsync(user, dto.Password);
//            if (!result.Succeeded)
//                return ApiResponse<string>.Error(string.Join("; ", result.Errors.Select(e => e.Description)));

//            await _signInManager.SignInAsync(user, isPersistent: false);
//            var token = GenerateToken(user);
//            return ApiResponse<string>.Ok(token);
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<string>.Error(ex.Message);
//        }
//    }

//    [HttpPost("login")]
//    public async Task<ApiResponse<string>> Login([FromBody] UserDto dto)
//    {
//        try
//        {
//            var user = await _userManager.FindByNameAsync(dto.Username);
//            if (user == null) return ApiResponse<string>.Error("Invalid credentials");

//            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
//            if (!result.Succeeded) return ApiResponse<string>.Error("Invalid credentials");

//            var token = GenerateToken(user);
//            return ApiResponse<string>.Ok(token);
//        }
//        catch (Exception ex)
//        {
//            return ApiResponse<string>.Error(ex.Message);
//        }
//    }

//    private string GenerateToken(IdentityUser<int> user)
//    {
//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//            new Claim(ClaimTypes.Name, user.UserName!)
//        };

//        var token = new JwtSecurityToken(
//            issuer: "yourapp",
//            audience: "yourapp",
//            claims: claims,
//            expires: DateTime.Now.AddHours(1),
//            signingCredentials: creds);

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}
