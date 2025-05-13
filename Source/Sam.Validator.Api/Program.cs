using Sam.Validator.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddCustomLocalization();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomLocalization();

app.MapControllers();

app.Run();
