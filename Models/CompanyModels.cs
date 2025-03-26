using System;
using System.Collections.Generic;

namespace PdfProcessingApi.Models
{
    public class Company
    {
        public string Rut { get; set; }
        public string Name { get; set; }
        public DateTime ConstitutionDate { get; set; }
        public DateTime CertificateIssueDate { get; set; }
        public string Domicile { get; set; }
        public decimal Capital { get; set; }
        public LegalRepresentative LegalRepresentative { get; set; }
    }

    public class LegalRepresentative
    {
        public string Name { get; set; }
        public string Rut { get; set; }
    }

    //public class Faculty
    //{
    //    public int Id { get; set; }
    //    public string Description { get; set; }
    //    public string Location { get; set; }
    //}
}
