using System;
using System.Collections.Generic;

namespace PdfProcessingApi.Models
{
    public class LegalReportResponse
    {
        public int ClientLegalReportRequestId { get; set; }
        public string ClientRut { get; set; }
        public string FacultyReportDate { get; set; }
        public string ValidityCertificateDate { get; set; }
        public int FacultyReportRecordYear { get; set; }
        public string SocietyReportDate { get; set; }
        public string ConstitutionDeedDate { get; set; }
        public string CompanyFantasyName { get; set; }
        public int SocietyReportRecordYear { get; set; }
        public string CompanyReportRecord { get; set; }
        public string CompanyCapital { get; set; }
        public string CompanyObjective { get; set; }
        public string CompanyDuration { get; set; }
        public string ElectronicRegistrationDate { get; set; }
        public string ElectronicVerificationCode { get; set; }
        public string ElectronicNotaryPersonName { get; set; }
        public string SocietyReportObservations { get; set; }
        public List<Representative> Representatives { get; set; }
    }
}