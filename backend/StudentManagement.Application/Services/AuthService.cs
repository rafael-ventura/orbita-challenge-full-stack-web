using Microsoft.Extensions.Configuration;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Helpers;
using StudentManagement.Application.Interfaces;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Exceptions;
using StudentManagement.Domain.Interfaces.Repositories;

namespace StudentManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly string _jwtSecret;
    private readonly int _jwtExpiryMinutes;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtSecret = configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret not found");
        _jwtExpiryMinutes = int.TryParse(configuration["JwtSettings:ExpiryMinutes"], out var min) ? min : 60;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.ExistsByEmailAsync(registerDto.Email))
            throw new InvalidStudentDataException($"Email '{registerDto.Email}' já está em uso.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = registerDto.Name.Trim(),
            Email = registerDto.Email.Trim().ToLower(),
            PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        // Token será gerado depois
        return new AuthResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = JwtHelper.GenerateToken(user, _jwtSecret, _jwtExpiryMinutes)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email.Trim().ToLower());
        if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new InvalidStudentDataException("E-mail ou senha inválidos.");

        // Token será gerado depois
        return new AuthResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = JwtHelper.GenerateToken(user, _jwtSecret, _jwtExpiryMinutes)
        };
    }
} 