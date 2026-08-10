using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentSystem.ViewModels
{
    public class BookAppointmentViewModel
    {
        [Required]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Display(Name = "Appointment Time")]
        public TimeSpan AppointmentTime { get; set; }

        [StringLength(500)]
        [Display(Name = "Reason for Visit")]
        public string? Reason { get; set; }
    }
}