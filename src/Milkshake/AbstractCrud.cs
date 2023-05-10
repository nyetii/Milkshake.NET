using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Milkshake.Models.Interfaces;

namespace Milkshake
{
    /// <summary>
    /// Interface for the needeed CRUD functions.
    /// </summary>
    public interface ICrud<in T> where T : class
    {
        Task<object?> GetMilkshake(Guid id);
        Task<object> GetAllMilkshakes();

        /// <summary>
        /// Creates a <see cref="IMilkshake"/> object and store its data somewhere.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="server"></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateMilkshake(T milkshake, ulong? server = null);
        Task UpdateMilkshake(T milkshake, Guid id);

        /// <summary>
        /// Deletes a <see cref="IMilkshake"/> object and any data related to it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/></returns>
        Task DeleteMilkshake(Guid id);
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
