using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrmiEyeHospital.Models
{
    [Table("contactusenquiry")]
    public class ContactUsEnquiry
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(200)]
        public string? Name { get; set; }

        [Column("email")]
        [MaxLength(200)]
        public string? Email { get; set; }

        [Column("contact_no")]
        [MaxLength(20)]
        public string? ContactNo { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("type")]
        public int Type { get; set; }   // existing: 1 = Contact, 2 = Enquiry

        [Column("status")]
        public int Status { get; set; } // existing: 0 = unread/pending, etc.

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // ---- NEW FIELDS ----
        [Column("is_urgent")]
        public bool IsUrgent { get; set; } = false;

        [Column("category")]
        [MaxLength(50)]
        public string? Category { get; set; }   // "Refractive", "Emergency", etc.
    }
}