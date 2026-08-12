using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Doctor
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .OrderBy(d => d.FullName)
                .ToListAsync();

            return View(doctors);
        }

        // GET: /Doctor/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartmentsAsync();

            return View();
        }

        // POST: /Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DoctorCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync(
                    model.DepartmentId);

                return View(model);
            }

            // Check if email already exists
            var existingUser =
                await _userManager.FindByEmailAsync(
                    model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account with this email already exists.");

                await LoadDepartmentsAsync(
                    model.DepartmentId);

                return View(model);
            }

            // Create Identity account
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(
                user,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                await LoadDepartmentsAsync(
                    model.DepartmentId);

                return View(model);
            }

            // Assign Doctor role
            await _userManager.AddToRoleAsync(
                user,
                "Doctor");

            // Create Doctor profile
            var doctor = new Doctor
            {
                UserId = user.Id,
                FullName = model.FullName,
                Specialization = model.Specialization,
                Qualification = model.Qualification,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                DepartmentId = model.DepartmentId
            };

            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Doctor created successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDepartmentsAsync(
            int? selectedDepartmentId = null)
        {
            var departments =
                await _context.Departments
                    .OrderBy(d => d.Name)
                    .ToListAsync();

            ViewBag.Departments =
                new SelectList(
                    departments,
                    "DepartmentId",
                    "Name",
                    selectedDepartmentId);
        }
    }
}