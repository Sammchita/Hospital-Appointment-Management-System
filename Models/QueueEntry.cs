namespace HospitalAppointmentSystem.Models
{
    public class QueueEntry
    {
        public int QueueEntryId { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; } = null!;

        public int QueueNumber { get; set; }

        public QueueStatus Status { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CalledTime { get; set; }

        public DateTime? CompletedTime { get; set; }
    }
}