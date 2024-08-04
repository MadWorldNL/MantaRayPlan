// See https://aka.ms/new-console-template for more information

var builder = WebApplication.CreateBuilder();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/healthz");

await app.RunAsync();

public partial class Program { }