using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchClass, string searchName)
        {
            List<Student> students = new List<Student>();

            try
            {
                var query = from s in _context.Students
                            select s;

                if (!string.IsNullOrEmpty(searchClass))
                {
                    query = query.Where(s => s.Class == searchClass);
                }

                if (!string.IsNullOrEmpty(searchName))
                {
                    query = query.Where(s => s.Name.Contains(searchName));
                }
                // Order by class
                query = query.OrderBy(s => s.Class);

                students = await query.ToListAsync();
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "An error occurred while fetching the student list.";
            }
            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            var classList = new List<string> { "Select Class" };
            classList.AddRange(StaticLists.Classes);
            ViewBag.Classes = new SelectList(classList);
            var genderList = new List<string> { "Select Gender" };
            genderList.AddRange(StaticLists.Genders);
            ViewBag.Genders = new SelectList(genderList);
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, IFormFile? imageUpload)
        {
            // Repopulate dropdown list in case of error
            var classList = new List<string> { "Select Class" };
            classList.AddRange(StaticLists.Classes);
            ViewBag.Classes = new SelectList(classList);
            var genderList = new List<string> { "Select Gender" };
            genderList.AddRange(StaticLists.Genders);
            ViewBag.Genders = new SelectList(genderList);
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageUpload != null && imageUpload.Length > 0)
                    {
                        // Define accepted image file types
                        string[] ACCEPTED_IMAGE_FILE_TYPES = { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };

                        // Extract file extension
                        var fileExtension = Path.GetExtension(imageUpload.FileName).ToLowerInvariant();
                        if (!ACCEPTED_IMAGE_FILE_TYPES.Contains(fileExtension))
                        {
                            string allowedTypes = string.Join(", ", ACCEPTED_IMAGE_FILE_TYPES);
                            ModelState.AddModelError("Image", $"Invalid file type. Only the following image types are allowed: {allowedTypes}.");
                        }
                        // Check if the file type is accepted
                        else Array.Exists(ACCEPTED_IMAGE_FILE_TYPES, ext => ext.Equals(fileExtension));
                        {
                            // Generate a unique filename
                            var fileName = $"{Guid.NewGuid()}{fileExtension}";

                            // Define paths
                            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                            var filePath = Path.Combine(directoryPath, fileName);

                            // Check if the directory exists, and create it if it doesn't
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            // Save the file
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageUpload.CopyToAsync(fileStream);
                            }

                            // Set the image file name for the student
                            student.Image = fileName;
                        }
                        
                    }

                    if (ModelState.IsValid)
                    {
                        _context.Students.Add(student);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                
                return View(student);
            }
            catch (DbUpdateException ex)
            {
                // Handle the exception
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null && sqlException.Number == 2627) // Unique constraint violation
                {
                    ModelState.AddModelError("", "A student with the same roll number and class already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving the student.");
                }
                return View(student);
            }
            catch (Exception exx)
            {
                ModelState.AddModelError("", $"An error occurred while saving the student. {exx.Message}");

                return View(student);
            }
        }

    }
}
