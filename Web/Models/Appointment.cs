using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrmiEyeHospital.Models
{
    [Table("appointment")]
    public class Appointment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("candidate_name")]
        [MaxLength(200)]
        public string? CandidateName { get; set; } = null;

        [Column("mobile_num")]
        [MaxLength(20)]
        public string? MobileNum { get; set; }

        [Column("email")]
        [MaxLength(200)]
        public string? Email { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("starttime")]
        public DateTime StartTime { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // ---- NEW FIELDS ----
        [Column("age")]
        public int? Age { get; set; }

        [Column("gender")]
        [MaxLength(20)]
        public string? Gender { get; set; }

        [Column("procedure_type")]
        [MaxLength(50)]
        public string? ProcedureType { get; set; }   // "OptiLASIK Standard", "Cataract Micro-Phaco", "Laser Screening", "Glaucoma Evaluation"

        [Column("confirmation_status")]
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Pending;
    }
}