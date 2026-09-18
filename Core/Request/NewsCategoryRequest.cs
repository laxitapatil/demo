namespace Core.Request
{
    public class NewsCategoryRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool Is_active { get; set; }
        public short? Order_by { get; set; }
    }
}