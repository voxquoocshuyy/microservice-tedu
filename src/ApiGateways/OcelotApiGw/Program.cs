using Common.Logging;
using Infrastructure.Middlewares;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information($"Starting {builder.Environment.ApplicationName} up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1");
            c.RoutePrefix = string.Empty;
        });
    }

    app.UseCors("CorsPolicy");

    app.UseMiddleware<ErrorWrappingMiddleware>();
    app.UseAuthentication();

    // app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync($"Hello voxquoocshuyy! This is {builder.Environment.ApplicationName}");
        });
    });
    app.MapControllers();

    await app.UseOcelot();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}