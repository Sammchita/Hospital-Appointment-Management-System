using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceptionistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // RECEPTIONIST DASHBOARD
        // GET: /Receptionist/Dashboard
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                .Where(a =>
                    a.AppointmentDate >= today &&
                    a.AppointmentDate < tomorrow)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            ViewBag.TotalAppointments =
                appointments.Count;

            ViewBag.PendingAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Pending);

            ViewBag.ConfirmedAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Confirmed);

            ViewBag.CheckedInAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.CheckedIn);

            ViewBag.CompletedAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Completed);

            ViewBag.CancelledAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Cancelled);

            return View(appointments);
        }


        // =========================================================
        // APPOINTMENT DETAILS
        // GET: /Receptionist/Details/5
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                .FirstOrDefaultAsync(a =>
                    a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            var viewModel =
                new ReceptionistAppointmentDetailsViewModel
                {
                    AppointmentId =
                        appointment.AppointmentId,

                    AppointmentDate =
                        appointment.AppointmentDate,

                    AppointmentTime =
                        appointment.AppointmentTime,

                    Reason =
                        appointment.Reason,

                    Status =
                        appointment.Status,

                    PatientId =
                        appointment.Patient.PatientId,

                    PatientName =
                        appointment.Patient.FullName,

                    PhoneNumber =
                        appointment.Patient.PhoneNumber,

                    Address =
                        appointment.Patient.Address,

                    DoctorId =
                        appointment.Doctor.DoctorId,

                    DoctorName =
                        appointment.Doctor.FullName,

                    Specialization =
                        appointment.Doctor.Specialization,

                    DepartmentName =
                        appointment.Doctor.Department.Name
                };

            return View(viewModel);
        }


        // =========================================================
        // CONFIRM APPOINTMENT
        // POST: /Receptionist/ConfirmAppointment
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(a =>
                        a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Only Pending appointments can be confirmed
            if (appointment.Status !=
                AppointmentStatus.Pending)
            {
                TempData["ErrorMessage"] =
                    "Only pending appointments can be confirmed.";

                return RedirectToAction(
                    nameof(Dashboard));
            }

            appointment.Status =
                AppointmentStatus.Confirmed;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment confirmed successfully.";

            return RedirectToAction(
                nameof(Dashboard));
        }


        // =========================================================
        // CHECK IN PATIENT
        // POST: /Receptionist/CheckIn
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id)
        {
            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(a =>
                        a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Patient should be confirmed before check-in
            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                TempData["ErrorMessage"] =
                    "Only confirmed appointments can be checked in.";

                return RedirectToAction(
                    nameof(Dashboard));
            }

            appointment.Status =
                AppointmentStatus.CheckedIn;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Patient checked in successfully.";

            return RedirectToAction(
                nameof(Dashboard));
        }


        // =========================================================
        // CANCEL APPOINTMENT
        // POST: /Receptionist/CancelAppointment
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(a =>
                        a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Completed and already cancelled appointments
            // cannot be cancelled.
            if (appointment.Status ==
                    AppointmentStatus.Completed ||
                appointment.Status ==
                    AppointmentStatus.Cancelled)
            {
                TempData["ErrorMessage"] =
                    "This appointment cannot be cancelled.";

                return RedirectToAction(
                    nameof(Dashboard));
            }

            appointment.Status =
                AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment cancelled successfully.";

            return RedirectToAction(
                nameof(Dashboard));
        }
    }
}