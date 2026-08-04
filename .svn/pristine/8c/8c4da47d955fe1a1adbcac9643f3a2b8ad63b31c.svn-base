using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class AspNetUsers : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_id { get; set; }
        public string Name { get; set; }
        public bool Is_active { get; set; }
        public string? Profile_image { get; set; }
        public DateTime Created_date { get; set; }
        public DateTime? Modified_date { get; set; }
        public int? Otp { get; set; }
        public DateTime? Otp_created_date { get; set; }
    }
}