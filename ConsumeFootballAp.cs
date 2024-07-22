using BeinConsoleAppStaj.Data;
using BeinConsoleAppStaj.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace BeinConsoleAppStaj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddDbContext<DataContext>();
                services.AddHostedService<ConsumeFootballApi>();
            });
    }
}


namespace BeinConsoleAppStaj
{
    public class ConsumeFootballApi : BackgroundService
    {
        private readonly ILogger<ConsumeFootballApi> _logger;
        private readonly HttpClient _httpClient;
        private readonly DataContext _dbContext;

        public ConsumeFootballApi(ILogger<ConsumeFootballApi> logger, HttpClient httpClient, DataContext dbContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _dbContext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker Running at :{time}", DateTimeOffset.Now);

                var request = new HttpRequestMessage(HttpMethod.Get, "https://v2.api-football.com/players/squad/541/2023");
                request.Headers.Add("X-RapidAPI-Key", "231a701173e4206fd4c3ddd95dfdb460");

                var response = await _httpClient.SendAsync(request, stoppingToken);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation("API Response: {data}", data);

                    var jsonDocument = JsonDocument.Parse(data);
                    var players = jsonDocument.RootElement.ToFootballPlayers();


                    if (!_dbContext.FootballPlayers.Any())
                    {
                        try
                        {
                            foreach (var player in players)
                            {
                                _dbContext.FootballPlayers.Add(player);
                            }

                            await _dbContext.SaveChangesAsync(stoppingToken);

                            _logger.LogInformation("Player data saved to database: {players}", players);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error saving player data to database");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Database already contains player data. Skipping insertion.");
                    }
                }
                else
                {
                    _logger.LogError("Error retrieving data from API: {statusCode}", response.StatusCode);
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}