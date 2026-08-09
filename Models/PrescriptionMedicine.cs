namespace HospitalAppointmentSystem.Models
{
    public class PrescriptionMedicine
    {
        public int PrescriptionMedicineId { get; set; }

        public int PrescriptionId { get; set; }

        public Prescription Prescription { get; set; }

        public string MedicineName { get; set; }

        public string Dosage { get; set; }

        public string Frequency { get; set; }

        public int DurationDays { get; set; }
    }
}
