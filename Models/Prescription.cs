namespace HospitalAppointmentSystem.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; }

        public string Diagnosis { get; set; }

        public string Instructions { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<PrescriptionMedicine> Medicines { get; set; }
    }
}
