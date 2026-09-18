namespace Core.Request
{
    public class TestimonialRequest
    {
        public int? Id { get; set; }
        public Guid Company_id { get; set; }
        public string? Author_name { get; set; }
        public string Comment { get; set; }
        public string? Designation { get; set; }
        public short Rating { get; set; }
        public string? Phone_number { get; set; }
        public string? Email { get; set; }
        public string? Treatment { get; set; }
    }
}