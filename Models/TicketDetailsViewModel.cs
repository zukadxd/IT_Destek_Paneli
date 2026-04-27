using System.Collections.Generic;

namespace IT_Destek_Panel.Models
{
    public class TicketDetailsViewModel
    {
        public Ticket Ticket { get; set; } = null!;
        public List<TicketMessage> Messages { get; set; } = new();
        public string? NewMessage { get; set; }
        public int? NewStatusId { get; set; } // Admin'in durumu değiştirebilmesi için ID tutuyoruz
    }
}