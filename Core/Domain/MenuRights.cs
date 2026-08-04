using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    [Table("menu_rights")]
    public class MenuRights
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("aspnet_role_id")]
        public string Role_id { get; set; }

        [Column("menu_id")]
        public int Menu_id { get; set; }

        [Column("modified_by")]
        public string Modified_by { get; set; }

        [Column("modified_date")]
        public DateTime Modified_date { get; set; }
    }
}