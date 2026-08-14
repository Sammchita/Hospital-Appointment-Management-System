using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Gender { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? EmergencyContact { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public ICollection<Prescription> Prescriptions { get; set; }
            = new List<Prescription>();
    }
}