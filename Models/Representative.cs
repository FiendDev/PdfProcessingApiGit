using System.Collections.Generic;


namespace PdfProcessingApi.Models
{
        public class Representative
        {
            public int Id { get; set; }
            public string RepresentativeRut { get; set; }
            public string RepresentativeName { get; set; }
            public string PersonType { get; set; }
            public string Date { get; set; }
            public string NotaryCity { get; set; }
            public string CreatedAt { get; set; }
            public string LastModifiedAt { get; set; }
            public string CompanyAdministrationTypeDescription { get; set; }
            public string Address { get; set; }
            public string RegionDescription { get; set; }
            public string CommuneDescription { get; set; }
            public string CityDescription { get; set; }
            public List<Faculty> Faculties { get; set; }
            public string DateString { get; set; }
        }
    }

