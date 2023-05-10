using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Attributes
{
    public class CrudAttribute : Attribute
    {
        public Type Type;

        public Type? GetType(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes();

                foreach (var item in attributes)
                {
                    if (item is CrudAttribute)
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}
