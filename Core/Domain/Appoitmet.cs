using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Domain
{
    [Table("appointments")]
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("company_id")]
        public Guid Company_id { get; set; }
        [Column("candidate_name")]
        public string Candidate_name { get; set; }
        [Column("mobile_num")]
        public string? Mobile_num { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("starttime")]
        public DateTime StartTime { get; set; }
        [Column("endtime")]
        public DateTime EndTime { get; set; }
        [Column("note")]
        public string? Note { get; set; }
        [Column("createddate")]
        public DateTime? CreatedDate { get; set; }
        [Column("createdby")]
        public short? CreatedBy { get; set; }
        [Column("modifieddate")]
        public DateTime? ModifiedDate { get; set; }
        [Column("modifiedby")]
        public short? ModifiedBy { get; set; }
    }
}