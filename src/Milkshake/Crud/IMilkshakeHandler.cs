using Milkshake.Models.Interfaces;

namespace Milkshake.Crud
{
    public interface IMilkshakeHandler<T, TSelf> where T : class
    where TSelf : class
    {
        Task<T?> Get(Guid id);
        Task<TSelf> GetMilkshake(Guid id);
        Task<T[]> Get(string name);
        Task<T[]> GetAll();
        Task<T[]> GetAll(Guid id);
        Task Add(T media);
        Task Update(T media, Guid id);
        Task Delete(T media);
        T Build();
    }
}
