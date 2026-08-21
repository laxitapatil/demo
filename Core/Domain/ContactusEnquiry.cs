using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Domain
{
    [Table("contactus_enquiry")]
    public class ContactusEnquiry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("company_id")]
        public Guid Company_id { get; set; }
        [Column("type")]
        public short Type { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("contact_no")]
        public string? Contact_no { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("status")]
        public short Status { get; set; }
        [Column("item_id")]
        public int? Item_id { get; set; }
        [Column("created_date")]
        public DateTime Created_date { get; set; }
    }
}