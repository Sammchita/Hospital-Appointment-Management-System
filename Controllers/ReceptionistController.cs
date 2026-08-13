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

        // GET: /Receptionist/Dashboard
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

            ViewBag.TotalAppointments = appointments.Count;

            ViewBag.PendingAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Pending);

            ViewBag.ConfirmedAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Confirmed);

            ViewBag.CompletedAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Completed);

            ViewBag.CancelledAppointments =
                appointments.Count(a =>
                    a.Status == AppointmentStatus.Cancelled);

            return View(appointments);
        }
        // GET: /Receptionist/Details/5
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

            var viewModel = new ReceptionistAppointmentDetailsViewModel
            {
                AppointmentId = appointment.AppointmentId,

                AppointmentDate = appointment.AppointmentDate,

                AppointmentTime = appointment.AppointmentTime,

                Reason = appointment.Reason,

                Status = appointment.Status,

                PatientId = appointment.Patient.PatientId,

                PatientName = appointment.Patient.FullName,

                DateOfBirth = appointment.Patient.DateOfBirth,

                PhoneNumber = appointment.Patient.PhoneNumber,

                Address = appointment.Patient.Address,

                DoctorId = appointment.Doctor.DoctorId,

                DoctorName = appointment.Doctor.FullName,

                Specialization = appointment.Doctor.Specialization,

                DepartmentName = appointment.Doctor.Department.Name
            };

            return View(viewModel);
        }
    }
}