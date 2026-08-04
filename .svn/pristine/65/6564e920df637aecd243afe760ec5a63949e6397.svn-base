using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    public class RegisterRequest
    {

        [Required(ErrorMessage = "Please enter Name.")]
        [StringLength(64, ErrorMessage = "Name length must be less than 64 character.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter Phone no.")]
        [StringLength(10, ErrorMessage = "Phone length must be 10 character.")]
        public string Phone_no { get; set; }


        [Required(ErrorMessage = "Please enter Email.")]
        [StringLength(128, ErrorMessage = "Email length must be 128 character.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password.")]
        public string Password { get; set; }

    }
}