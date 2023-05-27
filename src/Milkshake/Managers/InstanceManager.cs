using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Exceptions;
using Milkshake.Models.Interfaces;

namespace Milkshake.Managers
{
    public class InstanceManager<T> where T : class, IInstanceBase, new()
    {
        public T Instance = new();

        private readonly MilkshakeService _service;

        public InstanceManager(MilkshakeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adds a new VIP user on the specified Instance.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <returns>A new implementation of <see cref="IInstanceBase"/> including the updated content.</returns>
        /// <exception cref="PermissionDeniedException"></exception>
        public T AddVip(string user, ContextData context)
        {
            if (Permission.IsPermitted(Instance.Vips, context.Caller))
                Instance.Vips = Permission.Add(Instance.Vips, user);
            else
                throw new PermissionDeniedException();

            return Instance;
        }

        /// <summary>
        /// Removes a VIP user on the specified Instance.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <returns>A new implementation of <see cref="IInstanceBase"/> including the updated content.</returns>
        /// <exception cref="PermissionDeniedException"></exception>
        public T RemoveVip(string user, ContextData context)
        {
            if (Permission.IsPermitted(Instance.Vips, context.Caller))
                Instance.Vips = Permission.Remove(Instance.Vips, user);
            else
                throw new PermissionDeniedException();

            return Instance;
        }
    }
}
