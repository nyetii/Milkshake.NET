using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Milkshake.Managers;
using Milkshake.Models.Interfaces;

namespace Milkshake
{
    /// <summary>
    /// Interface for the needeed CRUD functions.
    /// </summary>
    public interface ICrud<in T> where T : class
    {
        Task<ContextData> GetContext(Guid id);
        Task<object?> GetMilkshake(Guid id);
        Task<object> GetAllMilkshakes(Guid? id = null);

        /// <summary>
        /// Creates a <see cref="IMilkshake"/> object and store its data somewhere.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="server"></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateMilkshake(T milkshake, ulong? server = null, bool save = true);
        Task UpdateMilkshake(T milkshake, Guid id, bool save = true);

        /// <summary>
        /// Deletes a <see cref="IMilkshake"/> object and any data related to it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/></returns>
        Task DeleteMilkshake(Guid id, bool save = true);

        Task SaveAsync();
    }


    //public abstract class AbstractCrud<T> : ICrud<T> where T : class
    //{
    //    public abstract Task<object?> GetMilkshake(Guid id);

    //    public abstract Task<object> GetAllMilkshakes();

    //    public abstract Task CreateMilkshake(T milkshake, ulong? server = null);

    //    public abstract Task UpdateMilkshake(Guid id);


    //    public abstract Task DeleteMilkshake(Guid id);
    //}
}
