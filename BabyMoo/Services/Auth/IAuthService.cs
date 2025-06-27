using BabyMoo.DTOs.Auth;

namespace BabyMoo.Service.AuthService
{
    public interface IAuthService
    {
        Task RegisterAsync(Register registerDto);
        Task<ResultDto> LoginAsync(Login loginDto);
    }
}
