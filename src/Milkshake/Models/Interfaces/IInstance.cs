namespace Milkshake.Models.Interfaces;

public interface IInstanceBase
{
    public Guid ContextId { get; set; }
    public string? Vips { get; set; }
}

public interface IInstance<TTemplate, TSource, TProperties> : IInstanceBase
    where TTemplate : class, ITemplate<TProperties>
    where TSource : class, ISource
    where TProperties : class, IProperties
{
    public ICollection<TTemplate>? Template { get; set; }
    public ICollection<TSource>? Source { get; set; }
}