using FluentValidation.AspNetCore;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Survey.Services;
using System.Reflection;
using Survey;
using Survey.Models;
using Microsoft.EntityFrameworkCore;
using Survey.Persestance;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddDependencies(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();
app.Run();