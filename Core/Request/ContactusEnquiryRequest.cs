namespace Core.Request
{
    public class ContactusEnquiryRequest
    {
        public int? Id { get; set; }
        public Guid Company_id { get; set; }
        public short Type { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contact_no { get; set; }
        public string Description { get; set; }
        public short? Status { get; set; }
        public int? Item_id { get; set; }
    }
}