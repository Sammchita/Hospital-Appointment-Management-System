using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.ViewModels
{
    public class ReceptionistDashboardViewModel
    {
        public int TotalAppointments { get; set; }

        public int PendingAppointments { get; set; }

        public int ConfirmedAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }

        public DateTime SelectedDate { get; set; }

        public List<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}