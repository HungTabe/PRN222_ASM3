using EventManagementProject.Data;
using EventManagementProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManagementProject.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using EventManagementProject.Hubs;

namespace EventManagementProject.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<EventHub> _hubContext;

        public EventController(ApplicationDbContext context, IHubContext<EventHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

        private bool IsUser()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "User";
        }

        // Hiển thị danh sách sự kiện
        public IActionResult Index(string searchString, int? categoryId, DateTime? startDate)
        {
            if (IsAdmin() || IsUser())
            {
                var events = _context.Events.Include(e => e.EventCategory).Include(e => e.Attendees).AsQueryable();
                if (!string.IsNullOrEmpty(searchString))
                    events = events.Where(e => e.Title.Contains(searchString));
                if (categoryId.HasValue)
                    events = events.Where(e => e.CategoryID == categoryId);
                if (startDate.HasValue)
                    events = events.Where(e => e.StartTime >= startDate);
                ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
                return View(events.ToList());
            }
            

            return Unauthorized();
        }

        // Tạo sự kiện mới (hiển thị form)
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
            return View(new CreateEventViewModel());
        }

        // Create event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEventViewModel model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            // Check if event title already exists
            if (_context.Events.Any(e => e.Title == model.Title))
            {
                ModelState.AddModelError("Title", "The event title already exists.");
            }

            // Check that end time is after start time
            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after the start time.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventEntity = new Event
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Location = model.Location,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        CategoryID = model.CategoryID
                        // No need to assign EventCategory or Attendees
                    };

                    _context.Events.Add(eventEntity);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "The event was created successfully.";
                    // Stay on the create page with an empty form
                    ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
                    return View(new CreateEventViewModel());
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error while creating the event: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to create the event. Please review the information and try again.";
            }

            // Keep the category dropdown and return view with current model
            ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
            return View(model);

        }


        // Chi tiết sự kiện
        // GET: /Events/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var eventEntity = _context.Events
                .Include(e => e.EventCategory)
                .Include(e => e.Attendees)
                .FirstOrDefault(e => e.EventID == id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            var model = new EventDetailViewModel
            {
                EventID = eventEntity.EventID,
                Title = eventEntity.Title,
                Description = eventEntity.Description ?? "",
                Location = eventEntity.Location ?? "",
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                CategoryName = eventEntity.EventCategory != null ? eventEntity.EventCategory.CategoryName : "No category",
                AttendeeCount = eventEntity.Attendees != null ? eventEntity.Attendees.Count : 0
            };

            return View(model);
        }

        // Deit event (hiển thị form)
        // GET: /Events/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var eventEntity = _context.Events.Find(id);
            if (eventEntity == null)
            {
                return NotFound();
            }

            var model = new EditEventViewModel
            {
                EventID = eventEntity.EventID,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Location = eventEntity.Location,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                CategoryID = eventEntity.CategoryID
            };

            ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
            return View(model);
        }

        // POST: /Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditEventViewModel model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            // Check for duplicate title (excluding current event)
            if (_context.Events.Any(e => e.Title == model.Title && e.EventID != model.EventID))
            {
                ModelState.AddModelError("Title", "Event title already exists.");
            }

            // Check if end time is after start time
            if (model.EndTime <= model.StartTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after start time.");
            }

            // Validate category exists
            if (model.CategoryID.HasValue && !_context.EventCategories.Any(c => c.CategoryID == model.CategoryID))
            {
                ModelState.AddModelError("CategoryID", "Selected category does not exist.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventEntity = _context.Events.Find(model.EventID);
                    if (eventEntity == null)
                    {
                        return NotFound();
                    }

                    // Update fields
                    eventEntity.Title = model.Title;
                    eventEntity.Description = model.Description;
                    eventEntity.Location = model.Location;
                    eventEntity.StartTime = model.StartTime;
                    eventEntity.EndTime = model.EndTime;
                    eventEntity.CategoryID = model.CategoryID;

                    _context.Events.Update(eventEntity);
                    _context.SaveChanges();

                    // Send real-time event update
                    var updatedEvent = _context.Events
                        .Include(e => e.EventCategory)
                        .Include(e => e.Attendees)
                        .FirstOrDefault(e => e.EventID == model.EventID);

                    var eventData = new EventDetailViewModel
                    {
                        EventID = updatedEvent.EventID,
                        Title = updatedEvent.Title,
                        Description = updatedEvent.Description ?? "",
                        Location = updatedEvent.Location ?? "",
                        StartTime = updatedEvent.StartTime,
                        EndTime = updatedEvent.EndTime,
                        CategoryName = updatedEvent.EventCategory != null ? updatedEvent.EventCategory.CategoryName : "No category",
                        AttendeeCount = updatedEvent.Attendees != null ? updatedEvent.Attendees.Count : 0
                    };
                    _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", model.EventID, eventData);

                    TempData["SuccessMessage"] = "Event updated successfully.";
                    // Stay on edit page with updated model
                    ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
                    return View(model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error updating event: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to update event. Please check the information.";
            }

            ViewBag.Categories = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
            return View(model);
        }

        // Xóa sự kiện (hiển thị xác nhận)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var @event = await _context.Events.Include(e => e.EventCategory).FirstOrDefaultAsync(m => m.EventID == id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
