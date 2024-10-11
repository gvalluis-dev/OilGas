using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OilGas.Data;
using OilGas.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173") // URL da aplica��o React
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Sensor Data API",
        Description = "Essa API disponibiliza dados e opera��es sobre eles de uma planta de �leo e G�s",
    });

    // Habilita a documenta��o via Swashbuckle
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



//Dependency Injection
builder.Services.AddScoped<SensorDataService>();

//AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program).Assembly);


//Initialize DBContext
builder.Services.AddDbContext<SensorContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurar o middleware CORS
app.UseCors("AllowReactApp"); // Use a pol�tica de CORS configurada

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
