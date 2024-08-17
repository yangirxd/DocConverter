using DocConverter.Application.Settings;
using Microsoft.Extensions.Hosting.WindowsServices;
using DocConverter.Application.DI;
using DocConverter.Infrastructure.DI;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                                     ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateSlimBuilder(options);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();
builder.Host.UseWindowsService();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppSettings.WorkingDirectory = Path.Combine(app.Environment.ContentRootPath, app.Configuration["TemporaryFolder"] ?? string.Empty);
AppSettings.AppConverter = app.Configuration["AppConverter"] ?? "libreoffice";
Directory.CreateDirectory(AppSettings.WorkingDirectory);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
