using System.ComponentModel.DataAnnotations;
using Milkshake.Generation;
using Milkshake.Media.Models;

namespace Milkshake.Instances;

public interface IMilkshakeInstance
{
    public string InstanceName { get; internal set; }

    public int TemplateCount => GetMilkshakeCount<Template>();
    public int SourceCount => GetMilkshakeCount<Source>();

    public int GenerationCount { get; internal set; }

    private int GetMilkshakeCount<T>() where T : class, IMilkshake, IMedia<T>
    {
        return 0;
    }

    public IGeneration CreateGeneration();
}