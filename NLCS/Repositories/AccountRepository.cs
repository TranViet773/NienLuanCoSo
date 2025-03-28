using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NLCS.Data;
using NLCS.Models.Entities;
using System.Security.Claims;

namespace NLCS.Repositories
{
    public class AccountRepository
    {
        public readonly ApplicationDbContext db;

        public AccountRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<string> GenerateAndSendVerificationCodeAsync(string email)
        {
            // Tạo mã xác nhận
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();

            // Soạn email
            var subject = "Mã xác nhận của bạn:";
            var message = $"{code}";
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Elearning", "tranquocviet07072003@gmail.com"));
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
                    await client.AuthenticateAsync("tranquocviet07072003@gmail.com", "keou evlh erya mhbu");

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

        public string getImageByUsername(string _username)
        {
            var user = db.Teachers.FirstOrDefault(u => u.Teacher_Username == _username);
            return user.Teacher_Image;
        }

        public Student getCurrentStudent(ClaimsPrincipal User)
        {
            var result = new Student();
            var emailStudentClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailStudentClaim != null)
            {
                var email = emailStudentClaim.Value;
                result = db.Students.FirstOrDefault(m => m.Student_Email == email);
            }
            return result;
        }
    }
}
