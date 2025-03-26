namespace PdfProcessingApi.Models;

public class PdfDataResponse
{
    public int ClientLegalReportRequestId { get; set; }
    public string ClientRut { get; set; }
    public DateTime FacultyReportDate { get; set; }
    // Todas las otras propiedades del JSON de salida
    // ...

    public List<Representative> Representatives { get; set; } = new();
}

//public class Representative
//{
//    public int Id { get; set; }
//    public string RepresentativeRut { get; set; }
//    public string RepresentativeName { get; set; }
//    // Otras propiedades...

//    public List<Faculty> Faculties { get; set; } = new();
//}


