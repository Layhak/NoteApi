using Microsoft.AspNetCore.Mvc;
using NotesApi.Data;
using NotesApi.Dto.User;
using NotesApi.Services;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly IAuthService _authService;

    public AuthController(IUsersRepository usersRepository, IAuthService authService)
    {
        _usersRepository = usersRepository;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserTokenDto>> Register(UserRegisterDto registerDto)
    {
        if (string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
            return BadRequest("Username and password are required");

        var existingUser = await _usersRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUser != null)
            return BadRequest("Username already exists");

        var passwordHash = _authService.HashPassword(registerDto.Password);
        var user = await _usersRepository.CreateAsync(registerDto.Username, passwordHash);

        var token = _authService.GenerateJwtToken(user);

        return Ok(new UserTokenDto
        {
            Id = user.Id,
            Username = user.Username,
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserTokenDto>> Login(UserLoginDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            return BadRequest("Username and password are required");

        var user = await _usersRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null)
            return Unauthorized("Invalid username or password");

        var isPasswordValid = _authService.VerifyPassword(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
            return Unauthorized("Invalid username or password");

        var token = _authService.GenerateJwtToken(user);

        return Ok(new UserTokenDto
        {
            Id = user.Id,
            Username = user.Username,
            Token = token
        });
    }

    [HttpGet("test-password")]
    public IActionResult TestPassword()
    {
        var password = "password123";
        var hash = _authService.HashPassword(password);
        var isValid = _authService.VerifyPassword(password, hash);

        return Ok(new {
            Hash = hash,
            IsValid = isValid,
            HashLength = hash.Length
        });
    }
}