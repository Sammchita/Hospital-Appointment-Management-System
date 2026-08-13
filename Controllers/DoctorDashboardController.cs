using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorDashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /DoctorDashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get logged-in Identity user
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
                return NotFound(
                    "Doctor profile could not be found.");
            }

            // Get today's appointments
            var today = DateTime.Today;

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctor.DoctorId &&
                    a.AppointmentDate.Date == today)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            var viewModel = new DoctorDashboardViewModel
            {
                Doctor = doctor,
                TodayAppointments = appointments
            };

            return View(viewModel);
        }
    }
}