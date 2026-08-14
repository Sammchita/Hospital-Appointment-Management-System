using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PrescriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PrescriptionController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Prescription/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Get currently logged-in patient
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Find patient profile
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (patient == null)
            {
                return NotFound("Patient profile not found.");
            }

            // Find prescription belonging to this patient
            var prescription = await _context.Prescriptions
                .Include(p => p.Doctor)
                    .ThenInclude(d => d.Department)
                .Include(p => p.Patient)
                .Include(p => p.Appointment)
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p =>
                    p.PrescriptionId == id &&
                    p.PatientId == patient.PatientId);

            if (prescription == null)
            {
                return NotFound(
                    "Prescription not found or you are not authorized to view it.");
            }

            return View(prescription);
        }
    }
}