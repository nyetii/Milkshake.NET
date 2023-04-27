using Milkshake.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InstanceAttribute : Attribute
    {
        
        public static Type? InstanceType(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes();

                foreach (var item in attributes)
                {
                    if (item is InstanceAttribute)
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }
}
