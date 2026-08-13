using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.ViewModels
{
    public class ConsultationViewModel
    {
        public int AppointmentId { get; set; }

        // Patient information
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        // Appointment information
        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public string? Reason { get; set; }

        // Consultation information
        [Required]
        [StringLength(1000)]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        [Display(Name = "Medicines / Prescription")]
        public string Medicines { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "Doctor's Notes")]
        public string? Notes { get; set; }
    }
}