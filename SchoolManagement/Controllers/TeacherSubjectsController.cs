using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class TeacherSubjectsController : Controller
    {
        private readonly SchoolContext _context;

        public TeacherSubjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: TeacherSubjects
        public async Task<IActionResult> Index()
        {
            var teacherSubjects = await (from ts in _context.TeacherSubjects
                                         join t in _context.Teachers on ts.TeacherId equals t.TeacherId
                                         join s in _context.Subjects on ts.SubjectId equals s.SubjectId
                                         select new TeacherSubjectDTO
                                         {
                                             TeacherId = t.TeacherId,
                                             TeacherName = t.Name,
                                             SubjectId = s.SubjectId,
                                             SubjectName = s.Name
                                         }).ToListAsync();

            return View(teacherSubjects);
        }

        // GET: TeacherSubjects/Create
        public IActionResult Create()
        {
            var teacherList = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Select Teacher" } };
            teacherList.AddRange(_context.Teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.Name }));
            ViewData["Teachers"] = new SelectList(teacherList, "Value", "Text");

            var subjectList = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Select Subject" } };
            subjectList.AddRange(_context.Subjects.Select(s => new SelectListItem { Value = s.SubjectId.ToString(), Text = s.Name }));
            ViewData["Subjects"] = new SelectList(subjectList, "Value", "Text");

            return View();
        }

        // POST: TeacherSubjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherSubjectRequestDTO request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isExist = await _context.TeacherSubjects
                        .AnyAsync(s => s.TeacherId == request.TeacherId && s.SubjectId == request.SubjectId);
                    if (isExist)
                    {
                        ModelState.AddModelError("", "This teacher is already assigned to this subject.");
                    }
                    else
                    {
                        var teacherSubject = new TeacherSubject
                        {
                            TeacherId = request.TeacherId,
                            SubjectId = request.SubjectId
                        };

                        _context.TeacherSubjects.Add(teacherSubject);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Unable to save changes. {ex.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while saving the TeacherSubject. {ex.Message}");
                }
            }

            var teacherList = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Select Teacher" } };
            teacherList.AddRange(_context.Teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.Name }));
            ViewData["Teachers"] = new SelectList(teacherList, "Value", "Text");

            var subjectList = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Select Subject" } };
            subjectList.AddRange(_context.Subjects.Select(s => new SelectListItem { Value = s.SubjectId.ToString(), Text = s.Name }));
            ViewData["Subjects"] = new SelectList(subjectList, "Value", "Text");

            return View(request);                     
        }
    }
}
