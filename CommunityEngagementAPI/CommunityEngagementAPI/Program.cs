using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentApp.API.Services;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HostingBackgroundService = Microsoft.Extensions.Hosting.BackgroundService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

//connect to the database
builder.Services.AddDbContext<CEAppContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("CEDbConnection")).UseSnakeCaseNamingConvention());
builder.Services.AddOutputCache(options =>{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(15)));
    options.AddPolicy("StatusPolicy", builder => builder.Expire(TimeSpan.FromMinutes(15)).Tag("status-tag"));
    
    });
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IVolunteerReportService, VolunteerReportService>();
builder.Services.AddScoped<ISdgReportService, SdgReportService>();
builder.Services.AddScoped<INgoReportService, NgoReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IIntelligenceService, IntelligenceService>();
builder.Services.AddScoped<IManagementReportService, ManagementReportService>();
//builder.Services.AddHostedService<BackgroundService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseSwagger();
    app.UseSwaggerUI();
//}

// Auto - apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CEAppContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception)
    {
        // Tables already exist from seed, ignore
    }
}
app.UseCors("AllowReact");
//app.UseHttpsRedirection();
app.UseOutputCache();
app.UseAuthorization();

app.MapControllers();

app.Run();
