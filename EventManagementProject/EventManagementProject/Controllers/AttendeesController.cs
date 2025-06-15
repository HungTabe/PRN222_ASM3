using EventManagementProject.Data;
using EventManagementProject.Hubs;
using EventManagementProject.Models;
using EventManagementProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EventManagementProject.Controllers
{
    public class AttendeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<EventHub> _hubContext;

        public AttendeesController(ApplicationDbContext context, IHubContext<EventHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterAttendeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = null;
                    if (int.TryParse(HttpContext.Session.GetString("UserId"), out int parsedUserId))
                    {
                        userId = parsedUserId;
                    }

                    var attendee = new Attendee
                    {
                        EventID = model.EventID,
                        UserID = userId,
                        Name = model.Name,
                        Email = model.Email,
                        RegistrationTime = DateTime.Now
                    };

                    _context.Attendees.Add(attendee);
                    _context.SaveChanges();

                    // Send real-time attendee count update
                    var attendeeCount = _context.Attendees.Count(a => a.EventID == model.EventID);
                    _hubContext.Clients.All.SendAsync("ReceiveAttendeeUpdate", model.EventID, attendeeCount);

                    TempData["SuccessMessage"] = "Registration successful.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error during registration: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to register. Please check the information.";
            }

            return RedirectToAction("Details", "Event", new { id = model.EventID });
        }
    }
}
