using System.Diagnostics;
using System.Net.Http;

namespace UpdownMonitoring.Services;

public class UpdownMonitoringService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UpdownMonitoringService> _logger;

    public UpdownMonitoringService(HttpClient httpClient, ILogger<UpdownMonitoringService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<bool> CheckUpDownStatus(string url)
    {
        try
        {
            HttpResponseMessage headReqResponse = await _httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Head, url)
            );

            headReqResponse.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
