using PdfProcessingApi.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfProcessingApi.Services
{
    public class FacultyDetector
    {

        private readonly List<FacultyDefinition> _facultyDefinitions = new()
        {
            // 7. COMPRAR Y VENDER BIENES MUEBLES, CORPORALES E INCORPORALES
            new FacultyDefinition
            {
                Id = 7,
                Description = "COMPRAR Y VENDER BIENES MUEBLES, CORPORALES E INCORPORALES",
                Patterns = new[]
                {
                    @"\b(?:celebrar\s+contratos\s+de\s+trabajo\b)",
                    @"\b(?:contratar\s+servicios\s+profesionales\b)",
                    @"\b(?:contratar\s+servicios\s+técnicos\b)",
                    @"\b(?:celebrar\s+contratos\s+de\s+obra\s+material\b)",
                    @"\b(?:celebrar\s+contratos\s+de\s+arrendamiento\b)"
                },
                Confidence = 0.9f
            },
            // 8. COBRAR Y PERCIBIR
            new FacultyDefinition
            {
                Id = 8,
                Description = "COBRAR Y PERCIBIR",
                Patterns = new[]
                {
                    @"\b(?:fijar\s+remuneraciones\b)",
                    @"\b(?:fijar\s+honorarios\b)",
                    @"\b(?:fijar\s+bonos\b)"
                },
                Confidence = 0.85f
            }
        };

        public List<Faculty> ExtractFaculties(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<Faculty>();

            var textNormalized = NormalizeText(text);
            var faculties = new ConcurrentBag<Faculty>();

            Parallel.ForEach(_facultyDefinitions, definition =>
            {
                foreach (var pattern in definition.Patterns)
                {
                    try
                    {
                        var matches = Regex.Matches(textNormalized, pattern, RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            faculties.Add(new Faculty
                            {
                                Id = definition.Id,
                                Description = definition.Description,
                                Confidence = definition.Confidence,
                                Location = CleanLocation(match.Value)
                            });
                        }
                    }
                    catch (RegexMatchTimeoutException) { continue; }
                }
            });

            return faculties.ToList();
        }

        private string CleanLocation(string text)
        {
            return text.Length > 100 ? text.Substring(0, 100) + "..." : text;
        }

        private string NormalizeText(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return Regex.Replace(sb.ToString(), @"\s+", " ").Trim();
        }
    }
}