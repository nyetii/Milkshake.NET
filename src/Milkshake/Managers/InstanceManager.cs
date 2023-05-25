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

        public T AddVip(string user, ContextData context)
        {
            if (Permission.IsPermitted(Instance.Vips, context.Caller))
                Instance.Vips = Permission.Add(Instance.Vips, user);
            else
                throw new PermissionDeniedException();

            return Instance;
        }

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
