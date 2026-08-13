using HospitalAppointmentSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalAppointmentSystem.Data
{
    public static class IdentitySeedData
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<
                    RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<
                    UserManager<ApplicationUser>>();

            string[] roles =
            {
                "Admin",
                "Doctor",
                "Receptionist",
                "Patient"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            var adminEmail = "admin@hospital.com";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    adminUser,
                    "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        adminUser,
                        "Admin");
                }
            }
            else
            {
                // Reset Admin password
                var token =
                    await userManager.GeneratePasswordResetTokenAsync(
                        adminUser);

                var resetResult =
                    await userManager.ResetPasswordAsync(
                        adminUser,
                        token,
                        "Admin@123");

                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }

                // Make sure Admin has Admin role
                if (!await userManager.IsInRoleAsync(
                        adminUser,
                        "Admin"))
                {
                    await userManager.AddToRoleAsync(
                        adminUser,
                        "Admin");
                }
            }
        }

            public static async Task AssignPatientRoleAsync(
    IServiceProvider serviceProvider)
        {
            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByEmailAsync(
                "patient@hospital.com");

            if (user != null)
            {
                if (!await userManager.IsInRoleAsync(user, "Patient"))
                {
                    await userManager.AddToRoleAsync(
                        user,
                        "Patient");
                }
            }
        }


    }
    }
