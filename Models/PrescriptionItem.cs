using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class PrescriptionItem
    {
        public int PrescriptionItemId { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        public Prescription Prescription { get; set; } = null!;

        [Required]
        [StringLength(200)]
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