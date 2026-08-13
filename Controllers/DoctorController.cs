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

        // GET: /Doctor/Dashboard
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<IActionResult> Dashboard()

        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Find doctor's profile
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.UserId == user.Id);

            if (doctor == null)
            {
                return NotFound("Doctor profile not found.");
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // Get today's appointments
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctor.DoctorId &&
                    a.AppointmentDate >= today &&
                    a.AppointmentDate < tomorrow)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            var viewModel = new DoctorDashboardViewModel
            {
                DoctorName = doctor.FullName,
                Specialization = doctor.Specialization,
                DepartmentName = doctor.Department.Name,

                TotalAppointmentsToday = appointments.Count,

                PendingAppointments = appointments.Count(
                    a => a.Status == AppointmentStatus.Pending),

                ConfirmedAppointments = appointments.Count(
                    a => a.Status == AppointmentStatus.Confirmed),

                CompletedAppointments = appointments.Count(
                    a => a.Status == AppointmentStatus.Completed),

                TodayAppointments = appointments
            };

            return View(viewModel);
        }

        // GET: /Doctor
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()

        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .OrderBy(d => d.FullName)
                .ToListAsync();

            return View(doctors);
        }

        // GET: /Doctor/Create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartmentsAsync();

            return View();
        }

        // GET: /Doctor/AppointmentDetails/5
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Find logged-in doctor's profile
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == user.Id);

            if (doctor == null)
            {
                return NotFound("Doctor profile not found.");
            }

            // Find appointment belonging to this doctor
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a =>
                    a.AppointmentId == id &&
                    a.DoctorId == doctor.DoctorId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            var viewModel = new DoctorAppointmentDetailsViewModel
            {
                AppointmentId = appointment.AppointmentId,

                AppointmentDate = appointment.AppointmentDate,

                AppointmentTime = appointment.AppointmentTime,

                Reason = appointment.Reason,

                Status = appointment.Status,

                PatientId = appointment.Patient.PatientId,

                PatientName = appointment.Patient.FullName,

                Gender = appointment.Patient.Gender,

                PhoneNumber = appointment.Patient.PhoneNumber,

                Address = appointment.Patient.Address
            };

            return View(viewModel);
        }


        // POST: /Doctor/Create
        [Authorize(Roles = "Admin")]
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