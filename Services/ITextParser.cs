using System;
using System.Collections.Generic;

namespace PdfProcessingApi.Services
{
    public interface ITextParser
    {
        string ExtractRut(string text);
        DateTime? ExtractDate(string text, string dateType);
        string ExtractCompanyName(string text);
        string ExtractCapital(string text);
        string ExtractCompanyObjective(string text);
        List<RepresentativeInfo> ExtractRepresentatives(string text);
        string ExtractVerificationCode(string text);
        string ExtractNotaryName(string text);
        string NormalizeText(string text);
    }

    public class RepresentativeInfo
    {
        public string Name { get; set; }
        public string Rut { get; set; }
        public string Address { get; set; }
    }
}
