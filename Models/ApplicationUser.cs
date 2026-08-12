using Microsoft.AspNetCore.Identity;

namespace HospitalAppointmentSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        internal string FullName;
    }
}