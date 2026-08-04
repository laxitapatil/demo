namespace Core.Request
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public int Otp { get; set; }
    }
}