using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using IT_Destek_Panel.Models;
using IT_Destek_Panel.Constants;
using ClosedXML.Excel;
using System.IO;

namespace IT_Destek_Panel.Controllers
{
    // Sadece Admin ve "2" (Admin Id) rolüne sahip olanlar girebilir
    [Authorize(Roles = "Admin, 2")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Yönetim Paneli Ana Sayfası (Dashboard)
        public IActionResult Index()
        {
            // İstatistikler için biletleri çekiyoruz
            var tickets = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.IsDeleted == false)
                .ToList();

            var viewModel = new AdminDashboardViewModel
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.StatusId == 1),       // Açık
                InProgressTickets = tickets.Count(t => t.StatusId == 2), // İşlemde
                ClosedTickets = tickets.Count(t => t.StatusId == 3),     // Kapalı
                Tickets = tickets.OrderByDescending(t => t.CreatedAt).ToList()
            };

            return View(viewModel);
        }

        // 2. Kullanıcı Yönetimi
        public IActionResult Users()
        {
            var users = _context.Users.Where(u => u.IsDeleted == false).ToList();
            return View(users);
        }

        // 3. Bilet Silme (Soft Delete)
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket != null)
            {
                ticket.IsDeleted = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 4. Kullanıcı Silme (Soft Delete)
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.SaveChanges();
                TempData["Success"] = SystemMessages.UserDeleted;
            }
            return RedirectToAction("Users");
        }

        // 5. Excel Raporu Oluşturma
        public IActionResult ExportToExcel()
        {
            // Veritabanından isimleri de alarak çekiyoruz
            var tickets = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.IsDeleted == false)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Destek Talepleri");

                // Başlıklar
                worksheet.Cell(1, 1).Value = "Talep ID";
                worksheet.Cell(1, 2).Value = "Kullanıcı";
                worksheet.Cell(1, 3).Value = "Başlık";
                worksheet.Cell(1, 4).Value = "Aciliyet";
                worksheet.Cell(1, 5).Value = "Durum";
                worksheet.Cell(1, 6).Value = "Oluşturma Tarihi";

                // Başlık Stil
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#2c3e50");
                headerRow.Style.Font.FontColor = XLColor.White;

                // Verileri Yazdır
                int row = 2;
                foreach (var item in tickets)
                {
                    worksheet.Cell(row, 1).Value = item.Id;
                    worksheet.Cell(row, 2).Value = item.User?.Username;
                    worksheet.Cell(row, 3).Value = item.Title;
                    worksheet.Cell(row, 4).Value = item.Priority?.Name; // .ToString() yerine .Name
                    worksheet.Cell(row, 5).Value = item.Status?.Name;   // .ToString() yerine .Name
                    worksheet.Cell(row, 6).Value = item.CreatedAt.ToString("dd.MM.yyyy HH:mm");
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Destek_Talepleri_Raporu_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx"
                    );
                }
            }
        }
    }
}