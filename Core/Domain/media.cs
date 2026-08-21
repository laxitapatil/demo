using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Domain
{
    [Table("media")]
    public class Media
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("company_id")]
        public Guid Company_id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("is_active")]
        public bool Is_active { get; set; }
        [Column("order_by")]
        public short? Order_by { get; set; }
        [Column("modified_date")]
        public DateTime Modified_date { get; set; }
    }
}