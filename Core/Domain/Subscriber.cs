using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    [Table("subscriber")]
    public class Subscriber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("is_active")]
        public bool Is_active { get; set; }
        [Column("created_date")]
        public DateTime Created_date { get; set; }
    }
}