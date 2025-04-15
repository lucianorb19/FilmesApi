using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;//builder.Services.AddSwaggerGen - DOCUMENTAÇÃO AUTOMÁTICA PELO SWAGGER

var builder = WebApplication.CreateBuilder(args);
//VARIÁVEL PARA DEFINIÇÕES ABAIXO
var connectionString = builder.Configuration.GetConnectionString("FilmeConnection");

builder.Services.AddDbContext<FilmeContext>(opts =>
opts.UseMySql(connectionString,
              ServerVersion.AutoDetect(connectionString)));

//AUTOMAPPER PODE SER USADO EM TODA A APLICAÇÃO
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();//HttpPatch

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FilmesApi", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
