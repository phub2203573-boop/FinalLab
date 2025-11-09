using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using HRDepartment.Models;
using HRDepartment.Data;

namespace HRDepartment.Controllers
{
    public class StaffController : Controller
    {
        private readonly StaffRepository _repo;
        private readonly IWebHostEnvironment _env;

        public StaffController(IWebHostEnvironment env)
        {
            _env = env;
            _repo = new StaffRepository(env.ContentRootPath);
        }

        public IActionResult Index()
        {
            var items = _repo.GetAll();
            return View(items);
        }

        public IActionResult Details(int id)
        {
            var s = _repo.GetById(id);
            if (s == null) return NotFound();
            return View(s);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff, IFormFile? Photo)
        {
            if (!ModelState.IsValid) return View(staff);

            // Prevent duplicate StaffId
            if (!string.IsNullOrWhiteSpace(staff.StaffId) && _repo.ExistsByStaffId(staff.StaffId))
            {
                ModelState.AddModelError(nameof(staff.StaffId), "Staff ID already exists.");
                return View(staff);
            }

            if (Photo != null && Photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var ext = Path.GetExtension(Photo.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await Photo.CopyToAsync(stream);
                }
                staff.PhotoPath = $"/uploads/{fileName}";
            }

            _repo.Add(staff);
            return RedirectToAction(nameof(Index));
        }
    }
}
