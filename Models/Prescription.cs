using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; } = null!;

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        [StringLength(1000)]
        public string? Diagnosis { get; set; }

        [StringLength(1000)]
        public string? Instructions { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<PrescriptionMedicine> Medicines { get; set; }
            = new List<PrescriptionMedicine>();
    }
}