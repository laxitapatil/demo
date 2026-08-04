using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    [Table("menu")]
    public class Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("caption")]
        public string Caption { get; set; }

        [Column("parent_id")]
        public int? Parent_id { get; set; }

        [Column("menu_type")]
        public short Menu_type { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("css_class")]
        public string? Css_class {  get; set; }

        [Column("order_by")]
        public short Order_by { get; set; }

        [Column("modified_by")]
        public string Modified_by { get; set; }

        [Column("modified_date")]
        public DateTime Modified_date { get; set; }
    }
}