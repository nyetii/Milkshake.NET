using System.Collections.Concurrent;
using System.Security.AccessControl;
using Microsoft.Extensions.Options;
using Milkshake.Configuration;
using Milkshake.Models;
using Milkshake.Models.Interfaces;

namespace Milkshake;

public class GenerationService
{
    //private readonly MilkshakeService _service;
    private readonly MilkshakeOptions _options;

    private readonly Queue<IGeneration> _promptQueue = [];

    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly ConcurrentQueue<Task> _taskQueue = [];

    public int QueueLength => _promptQueue.Count;

    public GenerationService(IOptions<MilkshakeOptions> options)
    {
        _options = options.Value;
    }

    public async Task EnqueueAsync(IGeneration prompt)
    {
        _promptQueue.Enqueue(prompt);
        _taskQueue.Enqueue(GenerateAsync());

        await ProcessQueueAsync(prompt);
    }

    private async Task GenerateAsync()
    {
        var prompt = _promptQueue.Dequeue();

        // Generation logic here.

        prompt.OnReady();

        await Task.CompletedTask;
    }


    private async Task ProcessQueueAsync(IGeneration prompt)
    {
        while (_promptQueue.Count > 1)
        {
            if (_promptQueue.TryPeek(out var firstPosition) && firstPosition == prompt)
                await _semaphore.WaitAsync();
            else
                break;
        }
        
        try
        {
            if (!_taskQueue.TryDequeue(out var generationTask))
            {
                lock (_promptQueue)
                {
                    _promptQueue.Dequeue();
                }

                return;
            }

            await generationTask;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}