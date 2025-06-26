using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserViewDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserViewDto>>(users);
        }

        public async Task<UserViewDto?> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? _mapper.Map<UserViewDto>(user) : null;
        }

        public async Task<bool> ToggleBlock(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Blocked = !user.Blocked;
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
