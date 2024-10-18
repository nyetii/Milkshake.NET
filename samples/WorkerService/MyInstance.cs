using Milkshake;
using Milkshake.Generation;
using Milkshake.Instances;
using Milkshake.Media.Models;

namespace WorkerService1;

public class MyInstance : InstanceBase
{
    private ILogger<MyInstance> _logger;
    public MyInstance(IMilkshakeService service, IGenerationService generation, IServiceProvider serviceProvider, ILogger<MyInstance> logger) 
        : base(service, generation, serviceProvider)
    {
        _logger = logger;
    }

    [Instance]
    private readonly string _instanceName = "works";

    public async Task Test()
    {
        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    if (_logger.IsEnabled(LogLevel.Information))
        //    {
        //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //    }
        //    await Task.Delay(1000, stoppingToken);
        //}

        var test = await Media.LoadAsync<Source>();

        var gen = Instance.CreateGeneration();
        var gen2 = Instance.CreateGeneration();
        var gen3 = Instance.CreateGeneration();

        gen.Ready += generation =>
        {
            _logger.LogInformation("First Ready from {instanceName}!", generation.Instance.Name);
        };
        
        gen2.Ready += generation =>
        {
            _logger.LogInformation("Second Ready from {instanceName}!", generation.Instance.Name);
        };

        gen3.Ready += generation =>
        {
            _logger.LogInformation("Third Ready from {instanceName}!", generation.Instance.Name);
        };

        Generation.Enqueue(gen);
        _logger.LogInformation(Generation.QueueLength.ToString());
        Generation.Enqueue(gen2);
        _logger.LogInformation(Generation.QueueLength.ToString());
        Generation.Enqueue(gen3);
        _logger.LogInformation(Generation.QueueLength.ToString());

        await Task.Delay(1000);
        _logger.LogInformation(Generation.QueueLength.ToString());
    }
}