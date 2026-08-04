using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    [Table("aspnet_user_logs")]
    public class AspNetUserLogs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("user_name")]
        public string User_name { get; set; }
        
        [Column("access_type")]
        public string Access_type { get; set; }
        
        [Column("location")]
        public string Location { get; set; }
        
        [Column("task_category")]
        public string Task_category { get; set; }
        
        [Column("status_code")]
        public int Status_code { get; set; }
        
        [Column("message")]
        public string Message { get; set; }
        
        [Column("created_date")]
        public DateTime Created_date { get; set; }
    }
}
