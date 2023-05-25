using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Managers
{
    public class Permission
    {
        public static bool IsPermitted(string? list, string id)
        {
            var vips = Serialize(list);

            return vips.Any(vip => vip == id);
        }

        internal static string Add(string? list, string id)
        {
            var vips = Serialize(list).ToList();
            vips.Add(id);
            return string.Join(';', vips);
        }

        internal static string Remove(string? list, string id)
        {
            var vips = Serialize(list).ToList();
            vips.RemoveAll(x => x == id);

            return string.Join(';', vips);
        }

        private static IEnumerable<string> Serialize(string? list)
        {
            var vips = list?.Split(';').AsEnumerable();
            if (vips is null)
                throw new InvalidOperationException("Vip list is null.");
            
            return vips;
        }
    }
}
