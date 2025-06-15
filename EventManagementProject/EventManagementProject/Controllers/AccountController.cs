using EventManagementProject.Data;
using EventManagementProject.Models;
using EventManagementProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Đăng ký
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra username đã tồn tại chưa
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Username has been existed.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    FullName = model.FullName,
                    Email = model.Email,
                    Role = "User" // Default User
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                // Lưu thông tin đăng nhập vào Session
                HttpContext.Session.SetString("UserId", user.UserID.ToString());
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Event");
            }
            return View(model);
        }

        // Đăng nhập
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Lưu thông tin đăng nhập vào Session
                    HttpContext.Session.SetString("UserId", user.UserID.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    return RedirectToAction("Index", "Event");
                }
                ModelState.AddModelError("", "Username or Password incorrect!.");
            }
            return View(model);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
