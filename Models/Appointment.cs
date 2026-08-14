using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        // Patient
        [Required]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        // Doctor
        [Required]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        // Appointment date and time
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        // Reason for appointment
        [StringLength(500)]
        public string? Reason { get; set; }

        // Appointment status
        [Required]
        public AppointmentStatus Status { get; set; }
            = AppointmentStatus.Pending;

        // When the appointment was created
        public DateTime CreatedAt { get; set; }
            = DateTime.Now;
        public Prescription? Prescription { get; set; }

    }
}