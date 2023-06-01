namespace Milkshake.Models.Interfaces;

public interface IInstanceBase : ICommonBase
{
    public Guid ContextId { get; set; }
    public string? Vips { get; set; }
}

public interface IInstance<TTemplate, TSource, TTopping> : IInstanceBase
    where TTemplate : class, ITemplate<TTopping>
    where TSource : class, ISource
    where TTopping : class, ITopping
{
    public ICollection<TTemplate>? Template { get; set; }
    public ICollection<TSource>? Source { get; set; }
}