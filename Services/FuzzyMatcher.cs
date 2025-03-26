namespace PdfProcessingApi.Services
{
    public class FuzzyMatcher
    {
        public string ExtractRut(string text)
        {
            // Implementar lógica de fuzzy matching para RUT
            var rutPattern = @"\d{1,2}\.\d{3}\.\d{3}-[\dKk]";
            var match = System.Text.RegularExpressions.Regex.Match(text, rutPattern);
            return match.Success ? match.Value : string.Empty;
        }

        // Añadir más métodos para extraer otros campos
        // ...
    }
}
