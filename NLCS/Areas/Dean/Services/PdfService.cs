using iTextSharp.text.pdf;
using iTextSharp.text;
using NLCS.Areas.Dean.ViewModels;
using iTextSharp.tool.xml;

namespace NLCS.Areas.Dean.Services
{
    public class PdfService : IPdfService
    {
        
        public string GenerateHtmlCertificate(CertificateViewModel certificateData)
        {
            // Đường dẫn đến file HTML template
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "CertificateTemplate.html");

            // Đọc nội dung HTML từ file template
            var htmlContent = File.ReadAllText(templatePath);

            // Thay thế các placeholder bằng dữ liệu từ CertificateViewModel
            htmlContent = htmlContent.Replace("{{StudentName}}", certificateData.StudentName)
                                     .Replace("{{CourseName}}", certificateData.CourseName)
                                     .Replace("{{NguoiCapBang}}", certificateData.NguoiCapBang)
                                     .Replace("{{ChucDanh}}", certificateData.ChucDanh)
                                     .Replace("{{RegistationDate}}", certificateData.RegistationDate.ToString("dd/MM/yyyy"))
                                     .Replace("{{CompletionDate}}", certificateData.CompletionDate.ToString("dd/MM/yyyy"));

            return htmlContent;
        }
        public byte[] GenerateCertificatePdf(string htmlContent)
        {
            using (var memoryStream = new MemoryStream())
            {
                //var document = new Document();
                //PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                //document.Open();
                Document document = new Document(PageSize.A4, 50, 50, 50, 50); // Set margins
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add border to the page
                PdfContentByte cb = writer.DirectContent;
                float borderWidth = 8f;
                cb.SetLineWidth(borderWidth);
                cb.SetRGBColorStroke(165, 42, 42); // Brown border color
                Rectangle pageSize = document.PageSize;
                cb.Rectangle(borderWidth / 2, borderWidth / 2, pageSize.Width - borderWidth, pageSize.Height - borderWidth);
                cb.Stroke();


                // Thêm logo vào đầu tài liệu PDF
                var logoUrl = "https://play-lh.googleusercontent.com/4GeT3f_7_qYLg_LiRYN_goUiO7z8ybHr0UxLYccr54rr7RZVEbg8OxESe-pFW4HzAzg"; // URL của hình ảnh logo
                var logo = Image.GetInstance(new Uri(logoUrl));
                logo.ScaleToFit(100f, 100f); // Kích thước của logo
                logo.Alignment = Element.ALIGN_CENTER;
                document.Add(logo);

                // Sử dụng XMLWorker để phân tích cú pháp HTML và CSS đơn giản
                using (var stringReader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }


                //PdfContentByte cb = writer.DirectContent;
                //float borderWidth = 8f;

                //cb.SetLineWidth(borderWidth);
                //cb.SetRGBColorStroke(165, 42, 42); // Black color for the border

                //Rectangle pageSize = document.PageSize;
                //cb.Rectangle(
                //    borderWidth / 2,
                //    borderWidth / 2,
                //    pageSize.Width - borderWidth,
                //    pageSize.Height - borderWidth
                //);

                //cb.Stroke();
                // Thêm chữ ký vào cuối tài liệu
                //var signature = Image.GetInstance("https://play-lh.googleusercontent.com/4GeT3f_7_qYLg_LiRYN_goUiO7z8ybHr0UxLYccr54rr7RZVEbg8OxESe-pFW4HzAzg"); // Đường dẫn đến chữ ký
                //signature.ScaleToFit(150f, 75f);
                //signature.Alignment = Element.ALIGN_CENTER;
                //document.Add(signature);

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }

}

