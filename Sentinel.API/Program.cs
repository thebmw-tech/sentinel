using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariables().Contains("SWAGGER_PROD") && Environment.GetEnvironmentVariable("SWAGGER_PROD") == "1")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Setup cors for local development
if (app.Environment.IsDevelopment())
{
    app.UseCors((corsPolicyBuilder) =>
    {
        corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
