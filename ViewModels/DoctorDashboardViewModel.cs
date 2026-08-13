using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.ViewModels
{
    public class DoctorDashboardViewModel
    {
        public string DoctorName { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public int TotalAppointmentsToday { get; set; }

        public int PendingAppointments { get; set; }

        public int ConfirmedAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public List<Appointment> TodayAppointments { get; set; }
            = new List<Appointment>();
        public Doctor Doctor { get; internal set; }
    }
}