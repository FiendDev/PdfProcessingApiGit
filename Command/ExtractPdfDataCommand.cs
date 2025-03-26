using MediatR;
using PdfProcessingApi.Models;

namespace PdfProcessingApi.Commands;

public class ExtractPdfDataCommand : IRequest<PdfDataResponse>
{
    public string S3FileKey { get; set; }
}