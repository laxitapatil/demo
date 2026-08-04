namespace Core.Response
{
    public class EmailResetResponse
    {
        public string ResetLink { set; get; }
        public string Name { set; get; }
        //public string Username { set; get; }
        public int Otp { set; get; }
        public bool IsAdmin { set; get; }
    }
}