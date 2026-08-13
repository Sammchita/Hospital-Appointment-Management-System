using HospitalAppointmentSystem.Models;

namespace HospitalAppointmentSystem.ViewModels
{
    public class DoctorDashboardViewModel
    {
        public Doctor Doctor { get; set; } = null!;

        public List<Appointment> TodayAppointments { get; set; }
            = new List<Appointment>();
    }
}