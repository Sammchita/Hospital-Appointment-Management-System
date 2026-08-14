using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; } = null!;

        [Required]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        [Required]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Diagnosis { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Medicines { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<PrescriptionItem> Items { get; set; }
            = new List<PrescriptionItem>();
    }
}