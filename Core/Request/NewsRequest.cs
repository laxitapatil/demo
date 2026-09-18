namespace Core.Request
{
    public class NewsRequest
    {
        public int? Id { get; set; }
        public Guid? Company_id { get; set; }
        public int News_category_id { get; set; }
        public string Title { get; set; }
        public string? Slug { get; set; }
        public string? Short_description { get; set; }
        public string? Description { get; set; }
        public string[]? Image { get; set; }
        public string? Tags { get; set; }
        public bool Is_active { get; set; }
        public bool Is_popular { get; set; }
        public short? Order_by { get; set; }
    }
}