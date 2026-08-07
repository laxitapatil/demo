namespace Core.Domain
{
    public class Appointment
    {
        public int Id { get; set; }
        public Guid Company_id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // add remaining columns: PatientName, Email, Phone, DoctorId, Status, Notes, CreatedOn, etc.
    }
}