using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        // Patient
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        // Doctor
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        // Appointment date and time
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        [StringLength(500)]
        public string? Reason { get; set; }

        public AppointmentStatus Status { get; set; }
            = AppointmentStatus.Scheduled;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}