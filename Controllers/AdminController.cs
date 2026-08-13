using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalPatients =
                    await _context.Patients.CountAsync(),

                TotalDoctors =
                    await _context.Doctors.CountAsync(),

                TotalDepartments =
                    await _context.Departments.CountAsync(),

                TotalAppointments =
                    await _context.Appointments.CountAsync(),

                PendingAppointments =
                    await _context.Appointments
                        .CountAsync(a =>
                            a.Status == AppointmentStatus.Pending),

                ConfirmedAppointments =
                    await _context.Appointments
                        .CountAsync(a =>
                            a.Status == AppointmentStatus.Confirmed),

                CompletedAppointments =
                    await _context.Appointments
                        .CountAsync(a =>
                            a.Status == AppointmentStatus.Completed),

                CancelledAppointments =
                    await _context.Appointments
                        .CountAsync(a =>
                            a.Status == AppointmentStatus.Cancelled),

                RecentAppointments =
                    await _context.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .ThenInclude(d => d.Department)
                        .OrderByDescending(a => a.CreatedAt)
                        .Take(10)
                        .ToListAsync()
            };

            return View(viewModel);
        }
    }
}