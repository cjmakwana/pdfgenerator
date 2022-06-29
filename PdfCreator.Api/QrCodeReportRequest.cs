using Newtonsoft.Json;

namespace PdfCreator.Api
{
    public class QrCodeReportRequest
    {
        [JsonProperty("qrCodedata")]
        public string QrCodeData { get; set; }
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }
        [JsonProperty("feederNumber")]
        public string FeederNumber { get; set; }
        [JsonProperty("customText")]
        public string CustomText { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
