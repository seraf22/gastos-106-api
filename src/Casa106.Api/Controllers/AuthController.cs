using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Casa106.Application.DTOs;
using Casa106.Infrastructure.Persistence;
using Casa106.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Casa106.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Casa106DbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(Casa106DbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("username y password requeridos");

        var user = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.User == request.Username);

        if (user == null)
            return Unauthorized("usuario o contraseña inválidos");

        // Verificar password con BCrypt
        var verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!verified)
            return Unauthorized("usuario o contraseña inválidos");

        // Generar JWT
        var jwtKey = _configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY") ?? "please-change-this-secret";
        var issuer = _configuration["Jwt:Issuer"] ?? "Casa106";
        var audience = _configuration["Jwt:Audience"] ?? "Casa106Client";
        var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var m) ? m : 60;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.User),
            new Claim(ClaimTypes.Name, user.User),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Rol ?? "User")
        };

        var key = new SymmetricSecurityKey(SHA256.HashData(Encoding.UTF8.GetBytes(jwtKey)));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new LoginResponse
        {
            Token = tokenString,
            ExpiresAt = expires,
            UserId = user.Id,
            Username = user.User,
            Email = user.Email,
            Role = user.Rol
        };

        return Ok(response);
    }
}
