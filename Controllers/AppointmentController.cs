using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Appointment/Book
        [HttpGet]
        public async Task<IActionResult> Book()
        {
            await LoadDepartmentsAsync();

            return View();
        }

        // POST: /Appointment/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(
            Appointment appointment)
        {
            // Get currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Find the patient's profile
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (patient == null)
            {
                TempData["ErrorMessage"] =
                    "Patient profile could not be found.";

                return RedirectToAction(
                    "Dashboard",
                    "Patient");
            }

            // Validate appointment date
            if (appointment.AppointmentDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    "AppointmentDate",
                    "Appointment date cannot be in the past.");
            }

            // Validate doctor
            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(
                    d => d.DoctorId == appointment.DoctorId);

            if (doctor == null)
            {
                ModelState.AddModelError(
                    "DoctorId",
                    "Please select a valid doctor.");
            }

            // Check whether doctor already has appointment
            var doctorAlreadyBooked =
                await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == appointment.DoctorId &&
                    a.AppointmentDate.Date ==
                        appointment.AppointmentDate.Date &&
                    a.AppointmentTime ==
                        appointment.AppointmentTime &&
                    a.Status != AppointmentStatus.Cancelled);

            if (doctorAlreadyBooked)
            {
                ModelState.AddModelError(
                    "",
                    "This doctor is already booked for the selected date and time.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync();
                return View(appointment);
            }

            // Connect appointment to logged-in patient
            appointment.PatientId = patient.PatientId;

            // Always start with Pending
            appointment.Status =
                AppointmentStatus.Pending;

            appointment.CreatedAt = DateTime.Now;

            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment booked successfully.";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetDoctors(
    int departmentId)
        {
            var doctors = await _context.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .OrderBy(d => d.FullName)
                .Select(d => new
                {
                    doctorId = d.DoctorId,
                    fullName = d.FullName,
                    specialization = d.Specialization
                })
                .ToListAsync();

            return Json(doctors);
        }

        // GET: /Appointment
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.UserId == user.Id);

            if (patient == null)
            {
                return NotFound(
                    "Patient profile not found.");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .ThenInclude(d => d.Department)
                .Where(a => a.PatientId == patient.PatientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();

            return View(appointments);
        }

        // POST: /Appointment/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(
                    p => p.UserId == user.Id);

            if (patient == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.AppointmentId == id &&
                    a.PatientId == patient.PatientId);

            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                TempData["ErrorMessage"] =
                    "A completed appointment cannot be cancelled.";

                return RedirectToAction(nameof(Index));
            }

            appointment.Status =
                AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment cancelled successfully.";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDepartmentsAsync(
            int? selectedDepartmentId = null)
        {
            var departments = await _context.Departments
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