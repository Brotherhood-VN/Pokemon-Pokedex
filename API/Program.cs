using API.Configurations;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add Cors
builder.Services.AddCors();

// Add Controllers
builder.Services.AddControllers();

// Setting DBContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add Authentication
builder.Services.AddAuthenticationConfigufation(builder.Configuration);

// Add Dependency Injection Services
builder.Services.AddDependencyInjectionConfiguration(typeof(Program));

// Swagger Config
builder.Services.AddSwaggerGenConfiguration();

builder.Services.AddSignalR();

//Exception Handling Middleware Error
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// add SPA
builder.Services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = @"wwwroot\AdminApp";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    await next();

    stopwatch.Stop();
    var elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request finished {elapsedSeconds}s", elapsedSeconds);
});

// add SPA
app.UseSpaStaticFiles();
app.UseSpa(spa =>
    {
        spa.Options.SourcePath = @"wwwroot\AdminApp";
    });

app.Run();
