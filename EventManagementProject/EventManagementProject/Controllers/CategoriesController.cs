using EventManagementProject.Data;
using EventManagementProject.Models;
using EventManagementProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagementProject.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }


        // GET: /Categories
        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            var categories = _context.EventCategories.Include(e => e.Events).ToList();
            return View(categories);
        }

        // GET: /Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            return View();
        }

        // POST: /Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCategoryViewModel model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var category = new EventCategory
                {
                    CategoryName = model.CategoryName
                    // No neet declate property Events
                };
                _context.EventCategories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Categories/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            var category = _context.EventCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            var model = new UpdateCategoryViewModel
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName
            };

            return View(model);
        }

        // POST: /Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateCategoryViewModel model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var category = _context.EventCategories.Find(model.CategoryID);
                if (category == null)
                {
                    return NotFound();
                }

                // Kiểm tra tên danh mục có trùng không (ngoại trừ danh mục hiện tại)
                if (_context.EventCategories.Any(c => c.CategoryName == model.CategoryName && c.CategoryID != model.CategoryID))
                {
                    ModelState.AddModelError("CategoryName", "The category name already exists.");
                    return View(model);
                }

                // Update CategoryName
                category.CategoryName = model.CategoryName;
                _context.EventCategories.Update(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Categories/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            var category = _context.EventCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: /Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }
            var category = _context.EventCategories.Find(id);
            if (category != null)
            {
                // Kiểm tra xem danh mục có sự kiện nào không
                if (!_context.Events.Any(e => e.CategoryID == id))
                {
                    _context.EventCategories.Remove(category);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Không thể xóa danh mục vì có sự kiện liên quan.");
                    return View(category);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Categories/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = _context.EventCategories
                .Include(c => c.Events)
                .FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        
    }
}
