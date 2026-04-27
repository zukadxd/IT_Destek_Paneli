using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using IT_Destek_Panel.Models;
using IT_Destek_Panel.Helpers;
using Microsoft.EntityFrameworkCore;

namespace IT_Destek_Panel.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // 1. Önce kullanıcıyı adına göre buluyoruz 
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.IsDeleted == false);

            if (user != null)
            {
                string inputHashed = SecurityHelper.HashPassword(password, user.PasswordSalt);

                // 2. Veritabanındaki şifreyle eşleşiyor mu?
                if (user.Password == inputHashed)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                    await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

                    if (user.Role == UserRole.Admin) return RedirectToAction("Index", "Admin");
                    return RedirectToAction("Index", "Ticket");
                }
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            // --- MİNİMUM 6, MAKSİMUM 20 KARAKTER KONTROLÜ ---
            if (password.Length < 6 || password.Length > 20)
            {
                ViewBag.Error = "Şifreniz en az 6, en fazla 20 karakter uzunluğunda olmalıdır!";
                return View();
            }

            // Kullanıcı adı zaten var mı kontrolü
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Bu kullanıcı adı sistemde zaten tanımlı.";
                return View();
            }

            // DİNAMİK ŞİFRELEME ADIMLARI:
            string salt = SecurityHelper.GenerateSalt(); // Rastgele tuz üret
            string hashedPass = SecurityHelper.HashPassword(password, salt); // Tuzlu şifre üret

            var newUser = new User
            {
                Username = username,
                Password = hashedPass,
                PasswordSalt = salt, // Tuzu saklıyoruz
                Role = UserRole.User,
                IsDeleted = false
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult NotFoundPage()
        {
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet]
        public IActionResult LoadWeather(string city, string lat, string lon)
        {
            return ViewComponent("WeatherWidget", new { city = city, lat = lat, lon = lon });
        }
    }
}