namespace HospitalAppointmentSystem.Models
{
    public class QueueEntry
    {
        public int QueueEntryId { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        public int QueueNumber { get; set; }

        public string Status { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CalledTime { get; set; }
    }
}
