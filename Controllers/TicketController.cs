using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IT_Destek_Panel.Models;
using System.Security.Claims;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;


namespace IT_Destek_Panel.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Kullanıcının Kendi Biletlerini Listelediği Ana Sayfa
        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));

            // .Include kullanarak durum ve aciliyet isimlerini (Lookup) çekiyoruz
            var tickets = _context.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.UserId == userId && t.IsDeleted == false)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return View(tickets);
        }

        // 2. Yeni Bilet Ekranını Getiren Metod (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // Dropdown listelerini veritabanından çekip sayfaya gönderiyoruz
            ViewBag.StatusList = new SelectList(_context.TicketStatuses.ToList(), "Id", "Name");
            ViewBag.PriorityList = new SelectList(_context.TicketPriorities.ToList(), "Id", "Name");

            return View();
        }

        // 3. Yeni Bilet Kaydetme Metodu (POST)
        [HttpPost]
        public async Task<IActionResult> Create(string title, string description, int priorityId, IFormFile? attachment)
        {
            // 1. Boş Alan Kontrolü
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            {
                ViewBag.Error = "Başlık ve mesaj alanları boş bırakılamaz!";
                ViewBag.StatusList = new SelectList(_context.TicketStatuses.ToList(), "Id", "Name");
                ViewBag.PriorityList = new SelectList(_context.TicketPriorities.ToList(), "Id", "Name");
                return View();
            }

            var userId = int.Parse(User.FindFirstValue("UserId"));
            string? filePath = null;
            DateTime? attachmentDate = null;

            // 2. Dosya Yükleme ve Presleme İşlemi
            if (attachment != null && attachment.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                var extension = Path.GetExtension(attachment.FileName).ToLower();

                if (allowedExtensions.Contains(extension))
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(attachment.FileName);
                    var exactPath = Path.Combine(uploadsFolder, uniqueFileName);

                    // IMAGESHARP PRES MAKİNESİ BURADA DEVREYE GİRİYOR 
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        // Resmi belleğe alıp işliyoruz
                        using (var image = await Image.LoadAsync(attachment.OpenReadStream()))
                        {
                            // Genişliği maksimum 1280px olacak şekilde orantılı küçült
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                Size = new Size(1280, 720)
                            }));

                            // Kaliteyi %75'e çekip kaydediyoruz (Boyut devasa düşer, kalite fark edilmez)
                            var encoder = new JpegEncoder { Quality = 75 };
                            await image.SaveAsync(exactPath, encoder);
                        }
                    }
                    else if (extension == ".pdf")
                    {
                        // PDF dosyaları resim olmadığı için eski yöntemle (olduğu gibi) kaydedilir
                        using (var stream = new FileStream(exactPath, FileMode.Create))
                        {
                            await attachment.CopyToAsync(stream);
                        }
                    }

                    filePath = "/uploads/" + uniqueFileName;
                    attachmentDate = DateTime.Now;
                }
                else
                {
                    ViewBag.Error = "Sadece .jpg, .png veya .pdf uzantılı dosyalar yükleyebilirsiniz.";
                    ViewBag.StatusList = new SelectList(_context.TicketStatuses.ToList(), "Id", "Name");
                    ViewBag.PriorityList = new SelectList(_context.TicketPriorities.ToList(), "Id", "Name");
                    return View();
                }
            }

            // 3. Veritabanı Kayıt İşlemi
            var ticket = new Ticket
            {
                Title = title,
                Description = description,
                PriorityId = priorityId,
                StatusId = 1, // Yeni açılan bilet her zaman "Açık" (1) durumdadır
                UserId = userId,
                CreatedAt = DateTime.Now,
                AttachmentPath = filePath,
                AttachmentDate = attachmentDate
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = IT_Destek_Panel.Constants.SystemMessages.TicketCreated;
            return RedirectToAction("Index");
        }

        // 4. Bilet Detayını ve Mesaj Geçmişini Getiren Metod (GET)
        public IActionResult Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var role = User.FindFirstValue(ClaimTypes.Role);

            var ticket = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .FirstOrDefault(t => t.Id == id && t.IsDeleted == false);

            if (ticket == null) return NotFound("Talep bulunamadı.");

            // Güvenlik: Admin değilse ve bilet başkasına aitse engelle
            if (role != "Admin" && role != "2" && ticket.UserId != userId)
                return Unauthorized("Bu işleme yetkiniz yok.");

            var messages = _context.TicketMessages
                .Include(m => m.User)
                .Where(m => m.TicketId == id && m.IsDeleted == false)
                .OrderBy(m => m.CreatedAt)
                .ToList();

            var viewModel = new TicketDetailsViewModel
            {
                Ticket = ticket,
                Messages = messages
            };
            // OKUNDU BİLGİSİ MOTORU 
            bool isUpdated = false;
            foreach (var msg in messages)
            {
                // Eğer mesajı başkası yazdıysa ve ben şu an sayfaya girdiysem okundu yap!
                if (msg.UserId != userId && !msg.IsRead)
                {
                    msg.IsRead = true;
                    isUpdated = true;
                }
            }

            // Eğer okunmamış mesaj yakalayıp true yaptıysak veritabanına kaydet
            if (isUpdated)
            {
                _context.SaveChanges();
            }
            return View(viewModel);
        }

        // 5. Yeni Mesaj Gönderme ve Durum Güncelleme Metodu (POST)
        [HttpPost]
        public IActionResult AddMessage(int ticketId, string newMessage, int? newStatusId)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var role = User.FindFirstValue(ClaimTypes.Role);

            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(newMessage))
            {
                var message = new TicketMessage
                {
                    TicketId = ticketId,
                    UserId = userId,
                    MessageBody = newMessage,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                _context.TicketMessages.Add(message);
            }

            // Admin Otonomasyonu ve Durum Değiştirme
            if (role == "Admin" || role == "2")
            {
                if (newStatusId.HasValue)
                {
                    ticket.StatusId = newStatusId.Value;
                }
                else if (ticket.StatusId == 1 && !string.IsNullOrWhiteSpace(newMessage))
                {
                    // Admin cevap yazdıysa durum "İşlemde" (2) olur
                    ticket.StatusId = 2;
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "İşleminiz başarıyla tamamlandı.";
            return RedirectToAction("Details", new { id = ticketId });
        }

        // 6. Kullanıcının Kendi Biletini Kapatması
        public IActionResult CloseTicket(int id)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (ticket != null)
            {
                ticket.StatusId = 3; // "Kapalı" durumu
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Bilet başarıyla kapatıldı.";
            }

            return RedirectToAction("Index");
        }
    }
}

