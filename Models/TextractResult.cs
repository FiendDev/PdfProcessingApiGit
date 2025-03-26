namespace PdfProcessingApi.Models;

public class TextractResult
{
    public List<TextractPage> Pages { get; set; } = new();
}

public class TextractPage
{
    public int PageNumber { get; set; }
    public string Text { get; set; }

    public (double width, double height) PageSize { get; set; }

    public PageDimensions Dimensions { get; set; }
}

public class PageDimensions
{
    public double Width { get; set; }
    public double Height { get; set; }
}