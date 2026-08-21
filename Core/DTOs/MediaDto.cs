namespace Urmieyecare.Core.DTOs
{
    public class MediaDto
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string? Remark { get; set; }
        public int OrderBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class MediaCreateDto
    {
        public string Path { get; set; } = null!;
        public string? Remark { get; set; }
        public int OrderBy { get; set; }
    }

    public class MediaUpdateDto
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string? Remark { get; set; }
        public int OrderBy { get; set; }
    }
}