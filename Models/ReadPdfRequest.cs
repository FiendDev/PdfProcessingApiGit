using System.Xml;

namespace PdfProcessingApi.Models
{
    public class ReadPdfRequest
    {
        public string S3Bucket { get; set; }
        public string S3Key { get; set; }
        public string ExtraString { get; set; }
    }


    public class PdfExtracData
    {
        public string UID { get; set; }
        public string idFacultad { get; set; }

        public string RutEmpresa { get; set; } 

        public string facultad { get; set; }
    
    }
}
