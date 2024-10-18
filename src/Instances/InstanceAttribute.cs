using System.Reflection;

namespace Milkshake;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
public class InstanceAttribute : Attribute
{
    public string Name { get; private set; }

    public InstanceAttribute(string name)
    {
        Name = name;
    }

    public InstanceAttribute()
    {

    }

    internal static InstanceAttribute? GetValue<T>(T caller) where T : class
    {
        var type = caller.GetType();

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var attribute = field.GetCustomAttribute<InstanceAttribute>();

            if (attribute is null) continue;

            attribute.Name = field.GetValue(caller) as string ?? "default";
            return attribute;
        }

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<InstanceAttribute>();

            if (attribute is null) continue;

            attribute.Name = property.GetValue(caller) as string ?? "default";
            return attribute;
        }

        if (type.GetCustomAttribute(typeof(InstanceAttribute), false) is InstanceAttribute attr
            && !string.IsNullOrWhiteSpace(attr.Name))
            return attr;

        return null;
    }
}