using AutoMapper;
using System.Net.Mail;
using System.Net;
using Core.Mailer;
using Core.Request;
using System.Xml.Linq;

namespace Api.Provider
{
    public class EmailProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ViewRender _viewRender;
        protected readonly IMapper mapper;

        public void Dispose() { }

        public EmailProvider(IConfiguration configuration, ViewRender viewRender, IMapper mapper)
        {
            _configuration = configuration;
            _viewRender = viewRender;
            mapper = mapper;
        }

        public async Task<bool> SendMail<T>(string to, string subject, string view, T model, Stream stream, string FileName)
        {
            try
            {
                SmtpClient client = new()
                {
                    Host = _configuration.GetValue<string>("SMTP:Host") ?? string.Empty,
                    Port = _configuration.GetValue<int>("SMTP:Port"),
                    EnableSsl = _configuration.GetValue<bool>("SMTP:EnableSsl"),
                    Credentials = new NetworkCredential()
                    {
                        UserName = _configuration.GetValue<string>("SMTP:UserEmail"),
                        Password = _configuration.GetValue<string>("SMTP:Password")
                    }
                };

                MailMessage message = new()
                {
                    Subject = subject,
                    Body = await _viewRender.Render<T>(view, model),
                    IsBodyHtml = true,

                    To = { to },
                    From = new MailAddress(_configuration.GetValue<string>("SMTP:UserEmail"),
                        _configuration.GetValue<string>("SMTP:Username")),
                };
                if (!string.IsNullOrWhiteSpace(to))
                    message.To.Add(to);

                IEnumerable<string> bccMails = _configuration.GetSection("SMTP:Bcc").Get<IEnumerable<String>>();
                if (bccMails != null)
                    foreach (string bccRecepient in bccMails)
                        message.Bcc.Add(bccRecepient);

                IEnumerable<string> ccMails = _configuration.GetSection("SMTP:Cc").Get<IEnumerable<String>>();
                if (ccMails != null)
                    foreach (string ccRecepient in ccMails)
                        message.CC.Add(ccRecepient);


                if (!string.IsNullOrEmpty(FileName))
                {
                    Attachment attachFile = new Attachment(stream, FileName);
                    message.Attachments.Add(attachFile);
                }

                await client.SendMailAsync(message);

                message.Attachments.Dispose();
                message.AlternateViews.Dispose();
                message.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ResetPassword(string email, string name, int otp)
        {
            try
            {
                ChangePasswordMail changePassword = new()
                {
                    Otp = otp.ToString(),
                    Name = name,
                    Icon = _configuration.GetValue<string>("ApiUrl") + "assets/img/logo.png",
                };

                return await SendMail(email, "Visa Passport - Forget Password",
                    _configuration.GetValue<string>("SMTP:ViewPath") + "ForgetPassword.cshtml",
                    changePassword, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendEmailContactUs(string email, string name, string mobile_no, string message, DateOnly createdDate)
        {
            try
            {
                string recipients = string.Join(",", _configuration.GetSection("SMTP:AdminEmail").Get<IEnumerable<string>>());

                if (recipients == null || !recipients.Any())
                    return false;

                SendContactUsEmail sendEmail = new()
                {
                    Email = email,
                    Name = name,
                    Message = message,
                    MobileNo = mobile_no,
                    Created_date = createdDate,
                };

                return await SendMail(recipients, "VisaPilot - Contact Us", _configuration.GetValue<string>("SMTP:ViewPath") + "ContactUs.cshtml", sendEmail, null, null);
            }
            catch
            {
                return false;
            }
        }
    }
}
