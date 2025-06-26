using BabyMoo.DTOs.User;

namespace BabyMoo.Services.User
{
 

  
        public interface IUserService
        {
            Task<List<UserViewDto>> GetAllUsers();
            Task<UserViewDto?> GetUserById(int id);
            Task<bool> ToggleBlock(int id);
        }
    }

    
    
