using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using WebPicTweak.API.Extensions;
using WebPicTweak.API.Notify;
using WebPicTweak.Application.Abstractions;
using WebPicTweak.Application.Options;
using WebPicTweak.Application.Services.ImageServices;
using WebPicTweak.Application.Services.ImageServices.ImageHandlersContext;
using WebPicTweak.Application.Services.ImageServices.ModifierProcess;
using WebPicTweak.Application.Services.ImageServices.Storage;
using WebPicTweak.Application.Services.UserServices;
using WebPicTweak.Application.Services.UserServices.Registration;
using WebPicTweak.Core.Abstractions.Image;
using WebPicTweak.Core.Abstractions.Log;
using WebPicTweak.Core.Models.Log;
using WebPicTweak.Core.Models.Users;
using WebPicTweak.Infrastructure;
using WebPicTweak.Infrastructure.Services;
using WebPicTweak.Infrastructure.Services.BackgroundServices;
using WebPicTweak.Infrastructure.Services.JWT;
using WebPicTweak.Infrastructure.Services.log;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter(
        renderMessage: true,
        formatProvider: null))
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
builder.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
}));

builder.Services.AddControllers();

builder.Services.AddScoped<IImageStorage, ImageStorage>();
builder.Services.AddSingleton<IImageProcessingBackgroundService, ImageProcessingBackground>();
builder.Services.AddHostedService<ImageProcessingBackgroundService>();

builder.Services.AddSingleton<ICleanupStorage, CleanupStorage>();
builder.Services.AddSingleton<ICleanupBackgroundService, CleanupBackgroundService>();
builder.Services.AddHostedService<BackgroundCleanup>();

builder.Services.AddScoped<IMainHandler, MainHandler>();
builder.Services.AddScoped<IImageHandlerFactory, ImageHandlerFactory>();
builder.Services.AddScoped<IArchiveBuilder, ArchiveBuilder>();
builder.Services.AddSingleton<ISessionStore, SessionStore>();
builder.Services.AddSignalR();
builder.Services.AddScoped<ILog, LogService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IProgressNotifier, ProgressNotifier>();
builder.Services.AddScoped<IImageModifierProcess, ImageModifierProcess>();


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddApiAutentication(builder.Services.BuildServiceProvider()
                .GetRequiredService<IOptions<JwtOptions>>());
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.Configure<StorageOptions>(options =>
{
    options.ImagesStorage = Environment.GetEnvironmentVariable("STORAGE_PATH") ?? "/localdata/images";
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestPath}";

    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.MapHub<ArchiveHub>("/archiveHub");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.SameAsRequest
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
