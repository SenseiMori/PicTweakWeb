using Microsoft.AspNetCore.HttpLogging;
using WebPicTweak.Application.Options;
using WebPicTweak.Application.Services.ImageServices;
using WebPicTweak.Application.Services.ImageServices.Info;
using WebPicTweak.Application.Services.ImageServices.RemoveExif.JPG;
using WebPicTweak.Core.Abstractions.Image;

var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod(); ;
}));

builder.Services.AddControllers();
builder.Services.AddScoped<IImageStorage, ImageStorage>();
builder.Services.AddScoped<IJpegInfo, JpegInfo>();
builder.Services.AddScoped<JPGFile>();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});
builder.Services.Configure<StorageOptions>(options =>
{
    options.ImagesStorage = Environment.GetEnvironmentVariable("STORAGE_PATH") ?? string.Empty;
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
