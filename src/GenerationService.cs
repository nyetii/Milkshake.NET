using System.Collections.Concurrent;
using System.Security.AccessControl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Milkshake.Configuration;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake;

public class GenerationService
{
    private readonly ILogger<GenerationService> _logger;

    private readonly MilkshakeOptions _options;

    private readonly Queue<IGeneration> _promptQueue = [];

    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly ConcurrentQueue<Func<Task>> _taskQueue = [];

    public int QueueLength => _promptQueue.Count;

    public GenerationService(IOptions<MilkshakeOptions> options, ILogger<GenerationService> logger)
    {
        _logger = logger;
        _options = options.Value;
    }

    public void Enqueue(IGeneration prompt)
    {
        _promptQueue.Enqueue(prompt);
        _taskQueue.Enqueue(GenerateAsync);

        //QueueLength++;

        Task.Run(async () => await ProcessQueueAsync(prompt)).ContinueWith(x =>
        {
            if (x is { IsFaulted: true, Exception: not null })
            {
                foreach (var ex in x.Exception.Flatten().InnerExceptions)
                {
                    _logger.LogError("{ex}", ex);
                }
            }
        });
    }

    private async Task GenerateAsync()
    {
        _logger.LogInformation("LENGTH IS {QueueLength}", QueueLength);
        var prompt = _promptQueue.Dequeue();
        _logger.LogInformation("LENGTH NOW IS {QueueLength}", QueueLength);
        //QueueLength--;

        // Generation logic here.

        prompt.Instance.GenerationCount++;

        prompt.OnReady();

        await Task.CompletedTask;
    }


    private async Task ProcessQueueAsync(IGeneration prompt)
    {
        if (_promptQueue.Count is 0)
            return;

        while (_promptQueue.Count > 0)
        {
            if (_promptQueue.TryPeek(out var firstPosition) && firstPosition != prompt)
                await _semaphore.WaitAsync();
            else if (_promptQueue.Count is 0)
                return;
            else
                break;
        }

        try
        {
            if (!_taskQueue.TryDequeue(out var generationTask))
            {
                if (_promptQueue.Count < _taskQueue.Count)
                    return;

                lock (_promptQueue)
                {
                    _promptQueue.Dequeue();
                    //QueueLength--;
                }

                return;
            }

            await generationTask();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}