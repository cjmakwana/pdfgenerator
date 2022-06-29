using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net;
using Wkhtmltopdf.NetCore;
using static System.Net.Mime.MediaTypeNames;
using Wkhtmltopdf.NetCore.Interfaces;
using Wkhtmltopdf.NetCore.Options;

namespace PdfCreator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private string qrCodeGenerator = "https://api.qrserver.com/v1/create-qr-code/?size={0}x{0}&data={1}";
        private readonly IGeneratePdf pdfGenerator;

        public ReportController(IGeneratePdf generatePdf)
        {
            pdfGenerator = generatePdf;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport([FromBody] QrCodeReportRequest request)
        {
            var image = await GenerateQrCode(request.QrCodeData, request.Size);
            var report = GeneratePrintReport(image, request);
            var pdf = pdfGenerator.GetPDF(report);

            if (pdf == null || pdf.Length == 0)
            {
                return NotFound();
            }

            return File(pdf, "application/pdf", $"sample.pdf");
        }

        private async Task<byte[]> GenerateQrCode(string data, int size)
        {
            using WebClient client = new();
            string url = string.Format(qrCodeGenerator, size, data);
            byte[] imageData = await client.DownloadDataTaskAsync(url);
            return imageData;
        }

        private string GeneratePrintReport(byte[] image, QrCodeReportRequest request)
{
            var base64Image = Convert.ToBase64String(image);
            string filler = string.Format("data:image/jpg;base64,{0}", base64Image);
            var report = CreateFromTemplate(request, filler);
            return report;
        }

        private string CreateFromTemplate(QrCodeReportRequest request, string filler)
        {
            var template = System.IO.File.ReadAllText("./Template.txt");
            var transformed = template.Replace("##qrCode##", filler).Replace("##serialNo##", request.SerialNumber).Replace("##feederNo##", request.FeederNumber).Replace("##customText##", request.CustomText);
            return transformed;
        }
    }
}
