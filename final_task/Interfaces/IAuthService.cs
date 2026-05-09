using final_task.Controllers;
using final_task.DTOs.Authitification;
using final_task.TokensDto;

namespace final_task.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto dto);
        Task<TokenDto> LoginAsync(LogInDto dto);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);

    }
}
