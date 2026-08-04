using Core.Enumeration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Request
{
    public class LoginRequest
    {
        /// <summary>
        /// (Required)  Username / Mobile no.
        /// </summary>
        /// <example>8849301885</example>
        [Required(ErrorMessage = "Please enter Username.")]
        [MaxLength(256, ErrorMessage = "Max length reached for Username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Password.")]
        public string Password { get; set; }

        public DeviceType Device_type { get; set; } = DeviceType.Android;
    }
}