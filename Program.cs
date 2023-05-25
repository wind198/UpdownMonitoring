using UpdownMonitoring.Jobs;
using UpdownMonitoring.Services;
using Quartz;

namespace UpdownMonitoring;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<UpdownMonitoringService>();

        builder.Services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            // Just use the name of your job that you created in the Jobs folder.
            var jobKey = new JobKey("CheckUpdown");
            q.AddJob<CheckUpdownStatusJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(
                opts =>
                    opts.ForJob(jobKey)
                        .WithIdentity("Run check updown status on schedule")
                        //This Cron interval can be described as "run every minute" (when second is zero)
                        .WithCronSchedule("0/10 * * ? * * *")
            );
        });
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        app.MapGet(
                "/weatherforecast",
                (HttpContext httpContext) =>
                {
                    var forecast = Enumerable
                        .Range(1, 5)
                        .Select(
                            index =>
                                new WeatherForecast
                                {
                                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                    TemperatureC = Random.Shared.Next(-20, 55),
                                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                                }
                        )
                        .ToArray();
                    return forecast;
                }
            )
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        app.Run();
    }
}
