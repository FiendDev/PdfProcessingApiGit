using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfProcessingApi.Services
{
    public class TextParser : ITextParser
    {
        public string ExtractRut(string text)
        {
            var match = Regex.Match(text, @"Rut (?:Sociedad|Empresa):\s*(?<rut>\d{1,2}\.\d{3}\.\d{3}-[\dkK])");
            return match.Success ? match.Groups["rut"].Value.Trim() : null;
        }

        public DateTime? ExtractDate(string text, string dateType)
        {
            var pattern = $@"{dateType}:\s*(?<day>\d{{1,2}})\s+de\s+(?<month>[a-z]+)\s+del?\s+(?<year>\d{{4}})";
            var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            var months = new Dictionary<string, int>
            {
                {"enero", 1}, {"febrero", 2}, {"marzo", 3}, {"abril", 4},
                {"mayo", 5}, {"junio", 6}, {"julio", 7}, {"agosto", 8},
                {"septiembre", 9}, {"octubre", 10}, {"noviembre", 11}, {"diciembre", 12}
            };

            if (months.TryGetValue(match.Groups["month"].Value.ToLower(), out int month))
            {
                return new DateTime(
                    int.Parse(match.Groups["year"].Value),
                    month,
                    int.Parse(match.Groups["day"].Value));
            }

            return null;
        }

        public string ExtractCompanyName(string text)
        {
            var match = Regex.Match(text, @"Razón Social:\s*(?<name>[^\n]+)");
            return match.Success ? match.Groups["name"].Value.Trim() : null;
        }

        public string ExtractCapital(string text)
        {
            var match = Regex.Match(text, @"capital (?:social)?\s*:\s*(?<capital>\$\d{1,3}(?:\.\d{3})*(?:,\d{2})?)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups["capital"].Value : null;
        }

        public string ExtractCompanyObjective(string text)
        {
            var match = Regex.Match(text, @"ARTÍCULO [^\w]*SEGUNDO[^\w]*OBJETO[^\w]*:\s*(?<objeto>[^\n]+)");
            return match.Success ? match.Groups["objeto"].Value.Trim() : null;
        }

        public List<RepresentativeInfo> ExtractRepresentatives(string text)
        {
            var representatives = new List<RepresentativeInfo>();
            var matches = Regex.Matches(text, @"(?<nombre>[A-ZÁÉÍÓÚÑ]+\s+[A-ZÁÉÍÓÚÑ\s]+),\s*Rut\s*(?<rut>\d{1,2}\.\d{3}\.\d{3}-[\dkK]),\s*domiciliado\s*en\s*(?<domicilio>[^\n;]+)");

            foreach (Match match in matches)
            {
                representatives.Add(new RepresentativeInfo
                {
                    Name = match.Groups["nombre"].Value.Trim(),
                    Rut = match.Groups["rut"].Value.Trim(),
                    Address = match.Groups["domicilio"].Value.Trim()
                });
            }

            return representatives;
        }

        public string ExtractVerificationCode(string text)
        {
            var match = Regex.Match(text, @"código de verificación electrónico \(CVE\) es:\s*(?<code>\w+)");
            return match.Success ? match.Groups["code"].Value.Trim() : null;
        }

        public string ExtractNotaryName(string text)
        {
            var match = Regex.Match(text, @"Firmado electrónicamente por\s+notario\s+(?<notario>[^\n]+)");
            return match.Success ? match.Groups["notario"].Value.Trim() : null;
        }

        public string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

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