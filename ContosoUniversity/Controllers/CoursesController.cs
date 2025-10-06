using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : BaseController
    {
        public CoursesController(SchoolContext context) : base(context)
        {
        }

        // GET: Courses
        public IActionResult Index()
        {
            var courses = _context.Courses.Include(c => c.Department);
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = _context.Courses.Include(c => c.Department).Where(c => c.CourseID == id).Single();
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name");
            return View(new Course());
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if an image is provided
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLower();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                        ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    // Validate file size (max 5MB)
                    if (teachingMaterialImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                        ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    try
                    {
                        // Create uploads directory if it doesn't exist
                        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "TeachingMaterials");
                        if (!Directory.Exists(uploadsPath))
                        {
                            Directory.CreateDirectory(uploadsPath);
                        }

                        // Generate unique filename
                        var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadsPath, fileName);

                        // Save file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            teachingMaterialImage.CopyTo(stream);
                        }
                        course.TeachingMaterialImagePath = $"/Uploads/TeachingMaterials/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("teachingMaterialImage", "Error uploading file: " + ex.Message);
                        ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }
                }

                _context.Courses.Add(course);
                _context.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("CourseID,Title,Credits,DepartmentID,TeachingMaterialImagePath")] Course course, IFormFile teachingMaterialImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if a new image is provided
                if (teachingMaterialImage != null && teachingMaterialImage.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    var fileExtension = Path.GetExtension(teachingMaterialImage.FileName).ToLower();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("teachingMaterialImage", "Please upload a valid image file (jpg, jpeg, png, gif, bmp).");
                        ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    // Validate file size (max 5MB)
                    if (teachingMaterialImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("teachingMaterialImage", "File size must be less than 5MB.");
                        ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }

                    try
                    {
                        // Create uploads directory if it doesn't exist
                        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "TeachingMaterials");
                        if (!Directory.Exists(uploadsPath))
                        {
                            Directory.CreateDirectory(uploadsPath);
                        }

                        // Generate unique filename
                        var fileName = $"course_{course.CourseID}_{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadsPath, fileName);

                        // Delete old file if exists
                        if (!string.IsNullOrEmpty(course.TeachingMaterialImagePath))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", course.TeachingMaterialImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Save new file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            teachingMaterialImage.CopyTo(stream);
                        }
                        course.TeachingMaterialImagePath = $"/Uploads/TeachingMaterials/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("teachingMaterialImage", "Error uploading file: " + ex.Message);
                        ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
                        return View(course);
                    }
                }

                _context.Entry(course).State = EntityState.Modified;
                _context.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course course = _context.Courses.Include(c => c.Department).Where(c => c.CourseID == id).Single();
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Course course = _context.Courses.Find(id);
            var courseTitle = course.Title;
            
            // Delete associated image file if it exists
            if (!string.IsNullOrEmpty(course.TeachingMaterialImagePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", course.TeachingMaterialImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Log the error but don't prevent deletion of the course
                    }
                }
            }
            
            _context.Courses.Remove(course);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}

