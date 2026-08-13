using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        // Appointment associated with this prescription
        [Required]
        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; } = null!;

        // Doctor who prescribed it
        [Required]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        // Patient who received it
        [Required]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        // Diagnosis
        [Required]
        [StringLength(1000)]
        public string Diagnosis { get; set; } = string.Empty;

        // Medicines / prescription
        [Required]
        [StringLength(2000)]
        public string Medicines { get; set; } = string.Empty;

        // Additional doctor's notes
        [StringLength(2000)]
        public string? Notes { get; set; }

        // Date of consultation
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}