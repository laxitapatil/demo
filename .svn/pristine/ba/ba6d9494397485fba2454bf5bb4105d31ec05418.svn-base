using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    public class MenuRequest
    {
        public int? Id { get; set; }


        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter Caption.")]
        public string Caption { get; set; }

        public int? Parent_id { get; set; }


        [Required(ErrorMessage = "Please enter Menu type.")]
        [Range(1, 3, ErrorMessage = "Menu type between 1 to 3.")]
        public short Menu_type { get; set; }

        public string? Url { get; set; }

        public string? Css_class { get; set; }


        [Required(ErrorMessage = "Please enter Order.")]
        public short Order_by { get; set; }
    }
}