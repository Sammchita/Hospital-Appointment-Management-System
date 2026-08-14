using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.ViewModels
{
    public class ConsultationViewModel
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Reason { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; } = string.Empty;

        public List<PrescriptionItemViewModel> PrescriptionItems { get; set; }
            = new List<PrescriptionItemViewModel>();
    }

    public class PrescriptionItemViewModel
    {
        [Required]
        [StringLength(200)]
        [Display(Name = "Medicine")]
        public string MedicineName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Dosage { get; set; }

        [StringLength(100)]
        public string? Frequency { get; set; }

        [StringLength(100)]
        public string? Duration { get; set; }

        [StringLength(500)]
        public string? Instructions { get; set; }
    }
}