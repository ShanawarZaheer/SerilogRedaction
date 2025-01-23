var builder = WebApplication.CreateBuilder(args);

/* Getting Configurations for redaction */
var sensitiveDataSettings = builder.Configuration.GetSection("SensitiveDataSettings").Get<SensitiveDataSettings>();
SensitiveDataRedactor.Initialize(sensitiveDataSettings);


/*Add Serilog configuration*/
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration) // Load from appsettings.json
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.With(new SensitiveDataEnricher()); // Register custom enricher
});


//Add services to the container.
builder.Services.AddSingleton(sensitiveDataSettings);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSerilogRequestLogging(); 
app.MapControllers();

app.Run();
