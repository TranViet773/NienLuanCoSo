using MimeKit;
using NLCS.Data;
using NLCS.Models.Entities;
using System.Security.Claims;

namespace NLCS.Areas.Admin.Repositories
{
    public class ManagerRepository
    {
        public readonly ApplicationDbContext db;

        public ManagerRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<string> GenerateAndSendVerificationCodeAsync(string email)
        {
            // Tạo mã xác nhận
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            // Soạn email
            var subject = "Mã xác nhận của bạn nè";
            var message = $"{code}";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your App Name", "vietb2111873@student.ctu.edu.vn"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            // Gửi email
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Kết nối đến máy chủ SMTP
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // Xác thực
                    await client.AuthenticateAsync("vietb2111873@student.ctu.edu.vn", "y8EmNxbk");

                    // Gửi email
                    await client.SendAsync(emailMessage);

                    // Ngắt kết nối
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                // Có thể log lỗi hoặc xử lý theo cách khác
                return "Lỗi";
                throw new Exception("Có lỗi xảy ra khi gửi email: " + ex.Message);
            }

            return code;
        }

        public Manager getCurrentManager(ClaimsPrincipal User)
        {
            var result = new Manager();
            var emailManagerClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailManagerClaim != null)
            {
                var email = emailManagerClaim.Value;
                result = db.Managers.FirstOrDefault(m => m.Manager_Email == email);
            }
            return result;
        }

    }
}
