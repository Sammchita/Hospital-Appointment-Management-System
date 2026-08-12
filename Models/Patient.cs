using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Patient
    {
        internal string Gender;
        internal string? EmergencyContact;

        public int PatientId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}