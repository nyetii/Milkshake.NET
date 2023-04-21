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
    /// Abstract declaration of the needeed CRUD functions.
    /// </summary>
    public abstract class AbstractCrud
    {
        /// <summary>
        /// Creates a <see cref="IMilkshake"/> object and store its data somewhere.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="server"></param>
        /// <returns><see cref="Task"/></returns>
        public abstract Task CreateMilkshake<T>(T milkshake, ulong? server = null) where T : class, IMilkshake;

        public abstract Task UpdateMilkshake<T>(T milkshake, Guid id) where T : class, IMilkshake;

        /// <summary>
        /// Deletes a <see cref="IMilkshake"/> object and any data related to it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/></returns>
        public abstract Task DeleteMilkshake<T>(T milkshake, Guid id) where T : class, IMilkshake;
    }
}
