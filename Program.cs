using Amazon.S3;
using Amazon.Textract;
using Microsoft.EntityFrameworkCore;
using PdfProcessingApi;
using PdfProcessingApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();



// Registrar AWS SDK para S3 y Textract
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonTextract>();

// Registra el parser de texto
builder.Services.AddScoped<ITextParser, TextParser>();

// Registra el detector de facultades
builder.Services.AddSingleton<FacultyDetector>();


builder.Services.AddScoped<IPdfProcessingService, PdfProcessingService>();
builder.Services.AddSingleton<FuzzyMatcher>();

// Registrar MediatR (se asume que los handlers están en el ensamblado actual)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());


// Configurar DbContext con autenticación de Windows
builder.Services.AddDbContext<EvaluacionDeClientesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EvaluacionDeClientesDb")));


// Agregar Swagger (opcional) para pruebas y documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware para desarrollo y producción
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
