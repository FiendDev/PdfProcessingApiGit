using MediatR;
using System.Collections.Generic;
using PdfProcessingApi.Models;

namespace PdfProcessingApi.CQRS.Queries
{
    public class ReadPdfQuery : IRequest<LegalReportResponse>
    {
        public string S3Bucket { get; set; }
        public string S3Key { get; set; }
        public string ExtraString { get; set; }
    }

    public class ReadPdfResult
    {
        public Company Company { get; set; }
        public List<Faculty> Faculties { get; set; }
    }
}
