using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        // Connection to ASP.NET Identity
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [Phone]
        public string? EmergencyContact { get; set; }

        // Patient can have many appointments
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}