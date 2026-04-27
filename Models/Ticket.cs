using System;
using System.Collections.Generic;

namespace IT_Destek_Panel.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //DURUM VE ACİLİYET BAĞLANTILARI
        public int StatusId { get; set; }
        public virtual TicketStatus? Status { get; set; }

        public int PriorityId { get; set; }
        public virtual TicketPriority? Priority { get; set; }

        //KULLANICI BİLGİLERİ 
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        //TARİH VE EKSTRALAR
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? AttachmentPath { get; set; }
        public DateTime? AttachmentDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        // İlişki: Bir bilete ait birden fazla mesaj (sohbet) olabilir
        public virtual ICollection<TicketMessage>? Messages { get; set; }
    }
}