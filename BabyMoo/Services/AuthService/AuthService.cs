using   BabyMoo.Data;
using BabyMoo.DTOs.Auth;
using BabyMoo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using BabyMoo.Service.AuthService;

namespace BabyMoo.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, IMapper mapper, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(Register regDto)
        {
            try
            {
                if (regDto == null)
                    throw new ArgumentNullException(nameof(regDto));

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == regDto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Registration failed: Email {regDto.Email} already exists.");
                    return false;
                }

                var user = _mapper.Map<User>(regDto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(regDto.Password);
                user.Salt = Guid.NewGuid();

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RegisterAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<ResultDto> LoginAsync(Login logDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == logDto.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Login failed: User with email {logDto.Email} not found.");
                    return new ResultDto { Error = "User not found" };
                }

                if (user.Blocked)
                {
                    _logger.LogWarning($"Login failed: User {logDto.Email} is blocked.");
                    return new ResultDto { Error = "User is blocked" };
                }

                if (!BCrypt.Net.BCrypt.Verify(logDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Login failed: Invalid password for {logDto.Email}.");
                    return new ResultDto { Error = "Invalid password" };
                }

                var token = GenerateJwtToken(user);

                return new ResultDto
                {
                    Id = user.Id,
                    Name = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LoginAsync: {ex.Message}");
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
