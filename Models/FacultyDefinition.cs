namespace PdfProcessingApi.Models
{
    public class FacultyDefinition
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string[] Patterns { get; set; }
        public string[] Synonyms { get; set; }
        public float Confidence { get; set; } = 0.8f;
    }
}
