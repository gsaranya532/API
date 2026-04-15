using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Repositories;
using WebAPI.Model;
using WebAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Service _service;
    private readonly IConfiguration _configuration;

    public AuthController(Service service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Registory model)
    {
        var exists = await _service.GetByUsernameAsync(model.Username);
        if (exists != null) return BadRequest("Username already exists");

        var user = await _service.RegisterUserAsync(model.Username, model.PasswordHash, model.Role);
        return Ok(new { user.Id, user.Name, user.Role });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login loginuser)
    {
        // Validate credentials
        var user = await _service.GetByUsernameAsync(loginuser.Username);
        // If user not found
        if (user == null)
            return Unauthorized("Invalid username or password");

        bool isValid = BCrypt.Net.BCrypt.Verify(loginuser.PasswordHash, user.PasswordHash);

        if (!isValid)
            return Unauthorized("Invalid username or password");


        // Create JWT token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, loginuser.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY"); // taking value fro mconfig file == ?_configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            return StatusCode(500, "JWT Key is missing.");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }
}
