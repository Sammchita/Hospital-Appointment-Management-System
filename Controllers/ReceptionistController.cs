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

        public ReceptionistController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Receptionist/Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard(
            DateTime? date)
        {
            var selectedDate =
                date?.Date ?? DateTime.Today;

            var appointments =
                await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .Where(a =>
                        a.AppointmentDate.Date ==
                        selectedDate)
                    .OrderBy(a => a.AppointmentTime)
                    .ToListAsync();

            var model =
                new ReceptionistDashboardViewModel
                {
                    SelectedDate = selectedDate,

                    TotalAppointments =
                        appointments.Count,

                    PendingAppointments =
                        appointments.Count(a =>
                            a.Status ==
                            AppointmentStatus.Pending),

                    ConfirmedAppointments =
                        appointments.Count(a =>
                            a.Status ==
                            AppointmentStatus.Confirmed),

                    CompletedAppointments =
                        appointments.Count(a =>
                            a.Status ==
                            AppointmentStatus.Completed),

                    CancelledAppointments =
                        appointments.Count(a =>
                            a.Status ==
                            AppointmentStatus.Cancelled),

                    Appointments = appointments
                };

            return View(model);
        }

        // POST: /Receptionist/Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(
            int id)
        {
            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(
                        a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                TempData["ErrorMessage"] =
                    "A cancelled appointment cannot be confirmed.";

                return RedirectToAction(
                    nameof(Dashboard));
            }

            appointment.Status =
                AppointmentStatus.Confirmed;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment confirmed successfully.";

            return RedirectToAction(
                nameof(Dashboard),
                new
                {
                    date = appointment.AppointmentDate
                });
        }

        // POST: /Receptionist/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(
            int id)
        {
            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(
                        a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                TempData["ErrorMessage"] =
                    "A completed appointment cannot be cancelled.";

                return RedirectToAction(
                    nameof(Dashboard));
            }

            appointment.Status =
                AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment cancelled successfully.";

            return RedirectToAction(
                nameof(Dashboard),
                new
                {
                    date = appointment.AppointmentDate
                });
        }

        // POST: /Receptionist/Complete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(
            int id)
        {
            var appointment =
                await _context.Appointments
                    .FirstOrDefaultAsync(
                        a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                TempData["ErrorMessage"] =
                    "A cancelled appointment cannot be completed.";

                return RedirectToAction(
                    nameof(Dashboard));
            }

            appointment.Status =
                AppointmentStatus.Completed;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Appointment marked as completed.";

            return RedirectToAction(
                nameof(Dashboard),
                new
                {
                    date = appointment.AppointmentDate
                });
        }
    }
}