using PdfProcessingApi.Models;


namespace PdfProcessingApi.Services;

public class PdfProcessingService : IPdfProcessingService
{
    private readonly FuzzyMatcher _fuzzyMatcher;

    public PdfProcessingService(FuzzyMatcher fuzzyMatcher)
    {
        _fuzzyMatcher = fuzzyMatcher;
    }

    public PdfDataResponse ProcessTextractResult(TextractResult textractResult)
    {
        var fullText = string.Join("\n", textractResult.Pages.Select(p => p.Text));

        // Implementar lógica de extracción usando fuzzy matching
        var response = new PdfDataResponse();

        // Ejemplo de extracción de RUT
        response.ClientRut = _fuzzyMatcher.ExtractRut(fullText);

        // Extraer otras propiedades usando patrones similares
        // ...

        return response;
    }
}