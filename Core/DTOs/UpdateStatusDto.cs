using Core.Enumeration;
namespace UrmiEyeHospital.DTOs
{
    public class UpdateConfirmationStatusDto
    {
        public ConfirmationStatus ConfirmationStatus { get; set; }
    }

    public class UpdateProcedureDto
    {
        public string? ProcedureType { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
    }

    public class UpdateEnquiryFlagsDto
    {
        public bool? IsUrgent { get; set; }
        public string? Category { get; set; }
    }
}