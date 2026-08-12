using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Doctors)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(departments);
        }

        // GET: /Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Department created successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // GET: /Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(department);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Department updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // GET: /Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Doctors)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Doctors)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            if (department.Doctors.Any())
            {
                TempData["ErrorMessage"] =
                    "This department cannot be deleted because it has doctors assigned to it.";

                return RedirectToAction(nameof(Index));
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Department deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}