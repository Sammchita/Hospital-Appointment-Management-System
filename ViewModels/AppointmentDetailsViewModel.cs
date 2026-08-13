using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.ViewModels
{
    public class ReceptionistAppointmentDetailsViewModel
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public string? Reason { get; set; }

        public AppointmentStatus Status { get; set; }

        // Patient
        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        // Doctor
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;
    }
}