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
    public interface ICrud
    {
        Task<object> GetAllMilkshakes<T>() where T : class, IMilkshake, new();
        Task<object> GetAllInstances();

        /// <summary>
        /// Creates a <see cref="IMilkshake"/> object and store its data somewhere.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="server"></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateMilkshake<T>(T milkshake, ulong? server = null) where T : class, IMilkshake;
        Task UpdateMilkshake<T>(Guid id) where T : class, IMilkshake, new();

        /// <summary>
        /// Deletes a <see cref="IMilkshake"/> object and any data related to it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/></returns>
        Task DeleteMilkshake<T>(Guid id) where T : class, IMilkshake, new();
    }


    public abstract class AbstractCrud : ICrud
    {
        public abstract Task<object> GetAllMilkshakes<T>() where T : class, IMilkshake, new();


        public abstract Task CreateMilkshake<T>(T milkshake, ulong? server = null) where T : class, IMilkshake;

        public abstract Task UpdateMilkshake<T>(Guid id) where T : class, IMilkshake, new();


        public abstract Task DeleteMilkshake<T>(Guid id) where T : class, IMilkshake, new();

        public abstract Task<object> GetAllInstances();
    }
}
