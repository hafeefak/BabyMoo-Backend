namespace BabyMoo.DTOs.User
{
    public class UserViewDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool Blocked { get; set; }
    }
}
