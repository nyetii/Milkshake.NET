using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models.Interfaces;

namespace Milkshake.Managers
{
    public class InstanceManager<T> where T : class, IInstanceBase, new()
    {
        public T Instance;

        private readonly MilkshakeService _service;

        public InstanceManager(MilkshakeService service)
        {
            _service = service;
        }

        public T AddVip(ulong user)
        {
            var vips = Instance.Vips?.Split(';').ToList();
            if (vips is null)
                throw new InvalidOperationException("Vip list is null.");

            vips.Add(user.ToString());
            Instance.Vips = string.Join(';', vips);

            return Instance;
        }

        public T RemoveVip(ulong user)
        {
            var vips = Instance.Vips?.Split(';').ToList();
            if (vips is null)
                throw new InvalidOperationException("Vip list is null.");

            vips.RemoveAll(id => id == user.ToString());

            Instance.Vips = string.Join(';', vips);

            return Instance;
        }
    }
}
