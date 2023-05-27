﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Milkshake.Managers;
using Milkshake.Models.Interfaces;

namespace Milkshake.Crud
{
    /// <summary>
    /// Interface for the needeed CRUD functions.
    /// </summary>
    public interface ICrud<in T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/> of <see cref="ContextData"/></returns>
        Task<ContextData> GetContext(Guid id);

        /// <summary>
        /// Returns a boxed Milkshake object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/> of <see cref="object"/></returns>
        Task<object?> GetMilkshake(Guid id);

        /// <summary>
        /// Returns a boxed array of every specified Milkshake .
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Task"/> of <see cref="object"/></returns>
        Task<object> GetAllMilkshakes(Guid? id = null);

        /// <summary>
        /// Creates a <see cref="IMilkshake"/> object and store its data somewhere.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="milkshake"></param>
        /// <param name="server"></param>
        /// <param name="save"></param>
        /// <returns><see cref="Task"/></returns>
        Task CreateMilkshake(T milkshake, ulong? server = null, bool save = true);

        /// <summary>
        /// Updates a <see cref="IMilkshake"/> or <see cref="IInstanceBase"/> object data.
        /// </summary>
        /// <param name="milkshake"></param>
        /// <param name="id"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        Task UpdateMilkshake(T milkshake, Guid id, bool save = true);

        /// <summary>
        /// Deletes a <see cref="IMilkshake"/> or <see cref="IInstanceBase"/> object and any data related to it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="save"></param>
        /// <returns><see cref="Task"/></returns>
        Task DeleteMilkshake(Guid id, bool save = true);

        Task SaveAsync();
    }
}
