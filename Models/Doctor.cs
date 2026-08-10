using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        // Connection to ASP.NET Identity
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Qualification { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // Department
        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;

        // Doctor can have many appointments
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}