using BabyMoo.DTOs.Auth;

namespace BabyMoo.Service.AuthService
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(Register registerDto);
        Task<ResultDto> LoginAsync(Login loginDto);
    }
}
