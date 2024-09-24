using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SchoolContext _context;

        public SubjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subjects.ToListAsync());
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            var classList = new List<string> { "Select Class" };
            classList.AddRange(StaticLists.Classes);
            ViewBag.Classes = new SelectList(classList);
            return View();
        }

        // POST: Subjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            var classList = new List<string> { "Select Class" };
            classList.AddRange(StaticLists.Classes);
            ViewBag.Classes = new SelectList(classList);
            try
            {
                if (ModelState.IsValid)
                {
                    var isExist = await _context.Subjects.AnyAsync(s => s.Name == subject.Name && s.Class == subject.Class);
                    if (isExist)
                    {
                        ModelState.AddModelError("", "A subject with the same name and class already exists.");
                    }
                    else
                    {
                        // Insert the new subject
                        _context.Subjects.Add(subject);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(subject);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving the Subject. {ex.Message}");

                return View(subject);
            }
        }

    }

}
