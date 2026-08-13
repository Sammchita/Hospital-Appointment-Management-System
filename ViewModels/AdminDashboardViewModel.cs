using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalPatients { get; set; }

        public int TotalDoctors { get; set; }

        public int TotalDepartments { get; set; }

        public int TotalAppointments { get; set; }

        public int PendingAppointments { get; set; }

        public int ConfirmedAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }

        public List<Appointment> RecentAppointments { get; set; }
            = new List<Appointment>();
    }
}