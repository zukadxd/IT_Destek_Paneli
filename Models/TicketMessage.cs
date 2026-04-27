using System;

namespace IT_Destek_Panel.Models
{
    public class TicketMessage
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket? Ticket { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public string MessageBody { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}