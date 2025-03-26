using PdfProcessingApi.Models;

namespace PdfProcessingApi.Services;

public interface IPdfProcessingService
{
    PdfDataResponse ProcessTextractResult(TextractResult textractResult);
}
