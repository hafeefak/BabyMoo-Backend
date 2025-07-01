using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Auth;
using BabyMoo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BabyMoo.Middleware;
using System.Security.Cryptography;

namespace BabyMoo.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public async Task RegisterAsync(Register registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new BadRequestException("User with this email already exists.");

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.Role = registerDto.Role ?? "User";

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ResultDto> LoginAsync(Login loginDto)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password");

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new ResultDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Token = token,
                RefreshToken = refreshToken.Token,
                Message = "Login successful"
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
        }

        // Refresh token method to create new JWT (optional)
        public async Task<ResultDto> RefreshAsync(string refreshToken)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken && r.ExpiresAt > DateTime.UtcNow && !r.IsRevoked));

            if (user == null)
                throw new UnauthorizedException("Invalid or expired refresh token");

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Optionally revoke old
            var oldToken = user.RefreshTokens.FirstOrDefault(r => r.Token == refreshToken);
            if (oldToken != null) oldToken.IsRevoked = true;

            user.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new ResultDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Token = newToken,
                RefreshToken = newRefreshToken.Token,
                Message = "Token refreshed"
            };
        }
    }
}
