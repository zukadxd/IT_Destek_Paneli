using System.Collections.Generic;

namespace IT_Destek_Panel.Models
{
    // Kullanıcı Rolleri
    public enum UserRole { User = 1, Admin = 2 }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public string PasswordSalt { get; set; } = string.Empty; // YENİ: Kullanıcıya özel dinamik 

        public UserRole Role { get; set; } = UserRole.User;
        public bool IsDeleted { get; set; } = false;

        // İlişkiler
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public virtual ICollection<TicketMessage>? Messages { get; set; }
    }
}