namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly MyInstance _instance;

    public Worker(ILogger<Worker> logger, IServiceProvider provider, MyInstance instance)
    {
        _logger = logger;
        _instance = instance;
    }

    public string InstanceName => "test";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _instance.Test();
    }
}
