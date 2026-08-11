using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class PrescriptionMedicine
    {
        public int PrescriptionMedicineId { get; set; }

        public int PrescriptionId { get; set; }

        public Prescription Prescription { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string MedicineName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Dosage { get; set; }

        [StringLength(100)]
        public string? Frequency { get; set; }

        [StringLength(100)]
        public string? Duration { get; set; }
    }
}