using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Milkshake.Models.Interfaces;

namespace Milkshake.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HandleAttribute : Attribute
    {
        public Type Type;

        public HandleAttribute(Type type)
        {
            Type = type.IsAssignableTo(typeof(IMilkshake))
                || type.GetCustomAttributes().Any(item => item is InstanceAttribute) ? type 
                : throw new NotSupportedException($"Type {type.ShortDisplayName()} must implement IMilkshake.");
        }
        
    }
}
