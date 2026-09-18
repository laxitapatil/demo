using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    [Table("testimonial")]
    public class Testimonial
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("company_id")]
        public Guid Company_id { get; set; }
        [Column("author_name")]
        public string? Author_name { get; set; }
        [Column("comment")]
        public string Comment { get; set; }
        [Column("designation")]
        public string? Designation { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Column("rating")]
        public short Rating { get; set; }
        [Column("phone_number")]
        public string? Phone_number { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("treatment")]
        public string? Treatment { get; set; }
        [Column("is_active")]
        public bool Is_active { get; set; }
        [Column("created_date")]
        public DateTime Created_date { get; set; }
        [Column("modified_date")]
        public DateTime? Modified_date { get; set; }
    }
}