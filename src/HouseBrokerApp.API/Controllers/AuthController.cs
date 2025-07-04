using HouseBrokerApp.API.MiddleWare;
using HouseBrokerApp.Contracts.Requests;
using HouseBrokerApp.Domain.Entities;
using HouseBrokerApp.Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtTokenService _jwtTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtTokenService jwtTokenService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return ResponseHelpers.BadRequestResponse("User already exists.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NormalizedEmail = request.Email.ToUpperInvariant(), 
            NormalizedUserName = request.Email.ToUpperInvariant(), 

            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Password =request.Password,
            Role = request.UserType
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            _logger.LogWarning("User registration failed for {Email}: {Errors}", request.Email, errors);
            return ResponseHelpers.BadRequestResponse("User registration failed", errors);
        }

        await _userManager.AddToRoleAsync(user, request.UserType);

        _logger.LogInformation("User registered: {Email}", request.Email);
        return Ok(new { Message = "Registration successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Email);

        if (user == null)
        {
            _logger.LogWarning("Login failed for {Email}: User not found", request.Email);
            return Unauthorized(new MiddleWare.ErrorResponse { Message = "Invalid credentials" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Login failed for {Email}: Incorrect password", request.Email);
            return Unauthorized(new MiddleWare.ErrorResponse { Message = "Invalid credentials" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user,roles);

        _logger.LogInformation("User logged in: {Email}", request.Email);
        return Ok(new { Token = token });
    }
}
