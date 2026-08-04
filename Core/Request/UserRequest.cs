using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    public class UserRequest
    {
        public string? Id { get; set; }


        //[Required(ErrorMessage = "Please enter Username")]
        //public string Username { get; set; }


        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        public string Email { get; set; }
        //public bool EmailConfirmed { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Qualification { get; set; }
        public string? Appointment_color { get; set; }


        [Required(ErrorMessage = "Please enter Active.")]
        public bool Is_active { get; set; }
        public List<string> Role { get; set; }
    }
}