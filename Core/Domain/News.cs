using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    [Table("news")]
    public class News
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("company_id")]
        public Guid? Company_id { get; set; }
        [Column("news_category_id")]
        public int News_category_id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("slug")]
        public string? Slug { get; set; }
        [Column("short_description")]
        public string? Short_description { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("image")]
        public string[]? Image { get; set; }
        [Column("tags")]
        public string? Tags { get; set; }
        [Column("is_active")]
        public bool Is_active { get; set; }
        [Column("is_popular")]
        public bool Is_popular { get; set; }
        [Column("order_by")]
        public short? Order_by { get; set; }
        [Column("created_date")]
        public DateTime? Created_date { get; set; }
        [Column("created_by")]
        public string? Created_by { get; set; }
        [Column("modified_date")]
        public DateTime? Modified_date { get; set; }
        [Column("modified_by")]
        public string? Modified_by { get; set; }
    }
}