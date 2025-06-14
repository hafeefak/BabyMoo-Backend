using BabyMoo.DTOs.Auth;

namespace BabyMoo.Service.AuthService
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(Register regDto);
        Task<ResultDto> LoginAsync(Login logDto);
    }
}

