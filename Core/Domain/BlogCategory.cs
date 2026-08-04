using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Domain
{
    [Table("blog_category")]
    public class BlogCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
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