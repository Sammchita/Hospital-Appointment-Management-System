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
    public class ConsultationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsultationController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Consultation/Create/5
        [HttpGet]
        public async Task<IActionResult> Create(int id)
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
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a =>
                    a.AppointmentId == id &&
                    a.DoctorId == doctor.DoctorId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Prevent consultation for cancelled appointment
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                TempData["ErrorMessage"] =
                    "A cancelled appointment cannot be consulted.";

                return RedirectToAction(
                    "Dashboard",
                    "Doctor");
            }

            // Check if consultation already exists
            var existingPrescription =
                await _context.Prescriptions
                    .FirstOrDefaultAsync(p =>
                        p.AppointmentId == appointment.AppointmentId);

            if (existingPrescription != null)
            {
                TempData["ErrorMessage"] =
                    "This appointment has already been consulted.";

                return RedirectToAction(
                    "Dashboard",
                    "Doctor");
            }

            var viewModel = new ConsultationViewModel
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = appointment.Patient.FullName,
                PhoneNumber = appointment.Patient.PhoneNumber,
                Reason = appointment.Reason,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,

                // Start with one medicine row
                PrescriptionItems = new List<PrescriptionItemViewModel>
                {
                    new PrescriptionItemViewModel()
                }
            };

            return View(viewModel);
        }

        // POST: /Consultation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ConsultationViewModel model)
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

            // Find appointment
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a =>
                    a.AppointmentId == model.AppointmentId &&
                    a.DoctorId == doctor.DoctorId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Don't allow consultation on cancelled appointment
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                ModelState.AddModelError(
                    "",
                    "A cancelled appointment cannot be consulted.");
            }

            // Check duplicate consultation
            var alreadyExists =
                await _context.Prescriptions
                    .AnyAsync(p =>
                        p.AppointmentId == appointment.AppointmentId);

            if (alreadyExists)
            {
                ModelState.AddModelError(
                    "",
                    "This appointment already has a consultation.");
            }

            if (!ModelState.IsValid)
            {
                model.PatientName = appointment.Patient.FullName;
                model.PhoneNumber = appointment.Patient.PhoneNumber;
                model.Reason = appointment.Reason;
                model.AppointmentDate = appointment.AppointmentDate;
                model.AppointmentTime = appointment.AppointmentTime;

                return View(model);
            }

            // Create prescription
            var prescription = new Prescription
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                PatientId = appointment.PatientId,
                Diagnosis = model.Diagnosis,
                CreatedAt = DateTime.Now
            };

            // Add prescription items
            foreach (var item in model.PrescriptionItems)
            {
                if (string.IsNullOrWhiteSpace(item.MedicineName))
                {
                    continue;
                }

                prescription.Items.Add(
                    new PrescriptionItem
                    {
                        MedicineName = item.MedicineName,
                        Dosage = item.Dosage,
                        Frequency = item.Frequency,
                        Duration = item.Duration,
                        Instructions = item.Instructions
                    });
            }

            // Create readable medicine summary
            var medicineSummary =
                prescription.Items
                    .Select(item =>
                        $"{item.MedicineName}" +
                        $"{(string.IsNullOrWhiteSpace(item.Dosage) ? "" : $" - {item.Dosage}")}" +
                        $"{(string.IsNullOrWhiteSpace(item.Frequency) ? "" : $" - {item.Frequency}")}" +
                        $"{(string.IsNullOrWhiteSpace(item.Duration) ? "" : $" - {item.Duration}")}")
                    .ToList();

            prescription.Medicines =
                medicineSummary.Count > 0
                    ? string.Join(Environment.NewLine, medicineSummary)
                    : null;

            // Add prescription
            _context.Prescriptions.Add(prescription);

            // Mark appointment as completed
            appointment.Status = AppointmentStatus.Completed;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Consultation and prescription saved successfully.";

            return RedirectToAction(
                "Dashboard",
                "Doctor");
        }
    }
}