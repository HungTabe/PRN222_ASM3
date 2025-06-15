using EventManagementProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }


        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var report = new
            {
                EventCount = _context.Events.Count(),
                AttendeeCount = _context.Attendees.Count(),
                EventsByCategory = _context.EventCategories
                    .Select(c => new { c.CategoryName, EventCount = c.Events.Count() })
                    .ToList()
            };
            return View(report);
        }
    }
}
