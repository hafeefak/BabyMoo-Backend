﻿using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false;

    
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
