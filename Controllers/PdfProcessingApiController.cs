using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using PdfProcessingApi.CQRS.Queries;
using PdfProcessingApi.Models;

namespace PdfProcessingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadPdfController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadPdfController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ReadPdf([FromBody] ReadPdfRequest request)
        {
            var query = new ReadPdfQuery
            {
                S3Bucket = "progarantia-textract-analysis",
                S3Key = request.S3Key,
                ExtraString = "request.ExtraString"
            };

            LegalReportResponse result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
