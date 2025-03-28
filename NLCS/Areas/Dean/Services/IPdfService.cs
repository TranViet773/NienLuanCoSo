using NLCS.Areas.Dean.ViewModels;

namespace NLCS.Areas.Dean.Services
{
    public interface IPdfService
    {
        byte[] GenerateCertificatePdf(string htmlContent);
        string GenerateHtmlCertificate(CertificateViewModel certificateData);
    }
}
