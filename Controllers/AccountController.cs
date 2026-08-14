using HospitalAppointmentSystem.Data;
using HospitalAppointmentSystem.Models;
using HospitalAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Account/RegisterPatient
        [HttpGet]
        public IActionResult RegisterPatient()
        {
            return View();
        }

        // POST: /Account/RegisterPatient
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPatient(
            PatientRegisterViewModel model)
        {
            // Check form validation
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check whether email already exists
            var existingUser =
                await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account with this email already exists.");

                return View(model);
            }

            // Create Identity user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(
                user,
                model.Password);

            // If Identity user creation fails
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            // Assign Patient role
            await _userManager.AddToRoleAsync(
                user,
                "Patient");

            // Create Patient profile
            var patient = new Patient
            {
                UserId = user.Id,
                FullName = model.FullName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmergencyContact = model.EmergencyContact
            };

            // Add Patient to database
            _context.Patients.Add(patient);

            // Save Patient profile
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Registration successful. Please login.";

            // Redirect to ASP.NET Identity Login
            return RedirectToPage(
                "/Account/Login",
                new
                {
                    area = "Identity"
                });
        }
    }
}