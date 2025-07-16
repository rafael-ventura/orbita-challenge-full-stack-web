using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Exceptions;

namespace StudentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await authService.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest(new { message = ErrorMessage.InvalidCredentials.GetDescription() });
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await authService.RegisterAsync(registerDto);
            return Created(string.Empty, result);
        }
        catch (Exception)
        {
            return BadRequest(new { message = ErrorMessage.UserAlreadyExists.GetDescription() });
        }
    }
} 