using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.ViewModels
{
    public class DoctorAppointmentDetailsViewModel
    {
        public int AppointmentId { get; set; }

        // Appointment information
        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public string? Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        // Patient information
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}