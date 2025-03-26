using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.Textract;
using Amazon.Textract.Model;
using Azure.Core;
using FuzzySharp;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PdfProcessingApi.CQRS.Queries;
using PdfProcessingApi.Models;
using PdfProcessingApi.Services;
using static System.Net.Mime.MediaTypeNames;
using static PdfProcessingApi.CQRS.Handlers.ReadPdfQueryHandler;

namespace PdfProcessingApi.CQRS.Handlers
{
    public class ReadPdfQueryHandler : IRequestHandler<ReadPdfQuery, LegalReportResponse>
    {

        private readonly IAmazonTextract _textractClient;
        private readonly EvaluacionDeClientesDbContext _dbContext; // Agregar DbContext
        private readonly FacultyDetector _facultyDetector;
        private readonly ITextParser _textParser;


        public ReadPdfQueryHandler( IAmazonTextract textractClient, EvaluacionDeClientesDbContext dbContext,ITextParser textParser, FacultyDetector facultyDetector)
        {
         
            
            _dbContext = dbContext; // Inyectar DbContext
            _textractClient = textractClient ?? throw new ArgumentNullException(nameof(textractClient));
            _facultyDetector = facultyDetector ?? throw new ArgumentNullException(nameof(facultyDetector));
            _textParser = textParser ?? throw new ArgumentNullException(nameof(textParser));

        }

        public async Task<LegalReportResponse> Handle(ReadPdfQuery request, CancellationToken cancellationToken)
        {
            
            // Procesar el texto extraído
            //var result = ProcessExtractedText(extractedText);


            var text = await ExtractTextWithTextractAsync(request.S3Bucket, request.S3Key, cancellationToken);

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException("El texto extraído del PDF está vacío");
            }

            return BuildLegalReport(text);


            // Insertar datos en la base de datos
            //await InsertPdfDataIntoDatabase(result, request.ExtraString);
        }

        private async Task InsertPdfDataIntoDatabase(LegalReportResponse result, string extraString)
        {
            // Generar UID único para este conjunto de datos
            string uid = Guid.NewGuid().ToString();

            // Crear lista de entidades para insertar
            var entitiesToInsert = new List<PdfExtracData>();


            var facultades = result.Representatives.FirstOrDefault().Faculties;
            var rut = result.ClientRut;
            rut = rut.Replace(".","").Replace("-", "");

            // Insertar datos de facultades
            foreach (var faculty in facultades)
            {
                entitiesToInsert.Add(new PdfExtracData
                {
                    UID = uid,
                    idFacultad = faculty.Id.ToString(),
                    facultad = faculty.Description,
                    RutEmpresa = rut
                });
            }

            // Insertar todos los datos en la base de datos
            await _dbContext.PdfExtracData.AddRangeAsync(entitiesToInsert);
            await _dbContext.SaveChangesAsync();
        }



        private LegalReportResponse BuildLegalReport(string text)
        {
            var normalizedText = _textParser.NormalizeText(text);
            var constitutionDate = _textParser.ExtractDate(normalizedText, "Fecha de Constitución");
            var emissionDate = _textParser.ExtractDate(normalizedText, "Fecha de Emisión");



            var representatives = _textParser.ExtractRepresentatives(normalizedText)
                .Select(r => new Representative
                {
                    RepresentativeName = r.Name,
                    RepresentativeRut = r.Rut,
                    Address = r.Address,
                    PersonType = "Natural",
                    CompanyAdministrationTypeDescription = "Administrador Conjunto",
                    Faculties = _facultyDetector.ExtractFaculties(normalizedText)
                }).ToList();


            return new LegalReportResponse
            {
                ClientRut = _textParser.ExtractRut(normalizedText),
                CompanyFantasyName = _textParser.ExtractCompanyName(normalizedText),
                ConstitutionDeedDate = constitutionDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                FacultyReportDate = emissionDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                CompanyCapital = _textParser.ExtractCapital(normalizedText),
                CompanyObjective = _textParser.ExtractCompanyObjective(normalizedText),
                CompanyDuration = "Por decisión de los socios",
                ElectronicVerificationCode = _textParser.ExtractVerificationCode(normalizedText),
                ElectronicNotaryPersonName = _textParser.ExtractNotaryName(normalizedText),
                Representatives = representatives
            };
        }


        private async Task<string> ExtractTextWithTextractAsync(string bucket, string key, CancellationToken cancellationToken)
        {
            var startRequest = new StartDocumentAnalysisRequest
            {
                DocumentLocation = new DocumentLocation
                {
                    S3Object = new Amazon.Textract.Model.S3Object
                    {
                        Bucket = "progarantia-textract-analysis",
                        Name = "CRTnfuwVXXnW.pdf"
                    }
                },
                FeatureTypes = new List<string> { "TABLES", "FORMS" }
            };

            var startResponse = await _textractClient.StartDocumentAnalysisAsync(startRequest, cancellationToken);
            var jobId = startResponse.JobId;

            GetDocumentAnalysisResponse analysisResponse;
            string status;
            do
            {
                await Task.Delay(5000, cancellationToken);
                analysisResponse = await _textractClient.GetDocumentAnalysisAsync(
                    new GetDocumentAnalysisRequest { JobId = jobId }, cancellationToken);
                status = analysisResponse.JobStatus;
            } while (status == "IN_PROGRESS" && !cancellationToken.IsCancellationRequested);

            var textBuilder = new StringBuilder();
            foreach (var block in analysisResponse.Blocks.Where(b => b.BlockType == "LINE"))
            {
                textBuilder.AppendLine(block.Text);
            }

            return textBuilder.ToString();
        }


        private LegalReportResponse ProcessExtractedText(string text)
        {
            // Normalizar texto para análisis
            text = _textParser.NormalizeText(text);

            // Extraer metadatos básicos
            var rut = _textParser.ExtractRut(text);
            var constitutionDate = _textParser.ExtractDate(text, "Fecha de Constitución");
            var emissionDate = _textParser.ExtractDate(text, "Fecha de Emisión");
            var companyName = _textParser.ExtractCompanyName(text);
            var capital = _textParser.ExtractCapital(text);
            var objective = _textParser.ExtractCompanyObjective(text);

            // Detectar facultades
            var faculties = _facultyDetector.ExtractFaculties(text);

            // Extraer representantes
            var representatives = _textParser.ExtractRepresentatives(text)
                .Select(r => new Representative
                {
                    RepresentativeName = r.Name,
                    RepresentativeRut = r.Rut,
                    Address = r.Address,
                    Faculties = faculties,
                    // Otros campos según necesidad
                }).ToList();

            return new LegalReportResponse
            {
                ClientRut = rut,
                CompanyFantasyName = companyName,
                ConstitutionDeedDate = constitutionDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                FacultyReportDate = emissionDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                CompanyCapital = capital,
                CompanyObjective = objective,
                Representatives = representatives,
                // Completa otros campos según tu modelo
            };
        }

     
    }

}