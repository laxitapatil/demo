using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        [Column("expires")]
        public DateTime Expires { get; set; }

        [Column("created_date")]
        public DateTime Created_date { get; set; }

        [Column("created_by_ip")]
        public string? Created_by_ip { get; set; }

        [Column("revoked")]
        public DateTime? Revoked { get; set; }

        [Column("revoked_by_ip")]
        public string? Revoked_by_ip { get; set; }

        [Column("replaced_by_token")]
        public string? Replaced_by_token { get; set; }

        public bool Is_expired => DateTime.UtcNow >= Expires;

        public bool Is_revoked => Revoked != null;

        public bool Is_active => !Is_revoked && !Is_expired;

        // FK to AspNetUsers
        [Column("aspnet_users_id")]
        public string? aspnet_users_id { get; set; }
        public AspNetUsers User { get; set; } = null!;
    }
}
