using AutoMapper;
using final_task.DTOs.Authitification;
using final_task.Interfaces;
using final_task.Models;
using final_task.TokensDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace final_task.Services
{
    public class AuthService : IAuthService
    {
        private readonly UsersContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(UsersContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public async Task<TokenDto> LoginAsync(LogInDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Email or Password is wrong");

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto) 
        {
            var user = _mapper.Map<User>(dto);
            user.IsActive = true;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var employeeRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Employee");
            if (employeeRole != null)
            {
                _context.UsersRoles.Add(new UsersRole
                {
                    UsersId = user.Id,
                    RoleId = employeeRole.Id
                });
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserDto>(user);
        }
        

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token is incorrect or expiered");

            var accessToken = GenerateJwtToken(user); 
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken, 
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),  
            };

            var roles = _context.UsersRoles
                .Where(ur => ur.UsersId == user.Id)
                .Include(ur => ur.Role)
                .Select(ur => ur.Role.Name)   
                .ToList();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,  
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes); 
        }
    }
}