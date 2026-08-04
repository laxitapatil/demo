using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Please enter New Password")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Please enter Old Password")]
        public string OldPassword { get; set; }
    }
}