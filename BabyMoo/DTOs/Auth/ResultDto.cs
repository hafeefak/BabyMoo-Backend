namespace BabyMoo.DTOs.Auth
{
    public class ResultDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; } = "User";
        public string? Error { get; set; }
        public string? Message { get; set; }


    }
}
