using UpdownMonitoring.Services;
using Quartz;

namespace UpdownMonitoring.Jobs;

public class CheckUpdownStatusJob : IJob
{
    UpdownMonitoringService _updownMonitoringService;

    public CheckUpdownStatusJob(UpdownMonitoringService updownMonitorService)
    {
        _updownMonitoringService = updownMonitorService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        string[] urls = new string[] { "https://niteco.com" };
        List<Task<bool>> tasks = new List<Task<bool>> { };
        foreach (string url in urls)
        {
            tasks.Add(_updownMonitoringService.CheckUpDownStatus(url));
        }

        Task.WaitAll(tasks.ToArray());
        return Task.FromResult(true);
    }
}
