using HardwareEmulator.Models;
using Microsoft.Extensions.Options;

namespace HardwareEmulator.Services
{
    public class EmulatorService : BackgroundService
    {
        private Guid? _id;
        private readonly NetSettings _netSettings;
        private readonly ILogger<EmulatorService> _logger;
        public EmulatorService(IOptions<NetSettings> netSettings, ILogger<EmulatorService> logger)
        {
            _id = null;
            _netSettings = netSettings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            for(; ; )
            {
                await Task.Delay(2000); // задержка
                if(_id != null)
                {
                    await Task.Delay(10000); // эмулируем работу по времени 10 секунд
                    try
                    {
                        using var client = new HttpClient();
                        var urlString = $"{_netSettings.Url}?id={_id.Value}";
                        var content = await client.GetStringAsync(urlString);
                        _logger.LogInformation($"send wen api get to {urlString}");
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, $"Error send wen api get _id => {_id}");
                    }
                    _id = null;
                }
            }
        }

        public async Task StartCharge(StartHardwareCharge request)
        {
            _id = request.RequestId;
        }
    }
}
