namespace Core.Request
{
    public class AppointmentRequest
    {
        public int? Id { get; set; }
        public Guid Company_id { get; set; }
        public string Candidate_name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
    }
}