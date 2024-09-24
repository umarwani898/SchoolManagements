using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SchoolContext _context;

        public TeachersController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher, IFormFile? imageUpload)
        {
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
                            teacher.Image = fileName;
                        }                        
                    }
                    if (ModelState.IsValid)
                    {
                        _context.Teachers.Add(teacher);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(teacher);
            }
            catch (DbUpdateException ex)
            {
                // Handle the exception
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null)
                {
                    ModelState.AddModelError("", "An error occurred while saving the teacher.");
                }

                return View(teacher);
            }
            catch (Exception exx)
            {
                ModelState.AddModelError("", $"An error occurred while saving the teacher. {exx.Message}");

                return View(teacher);
            }
            
        }

    }

}
