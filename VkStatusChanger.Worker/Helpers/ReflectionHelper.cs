using System.Reflection;

namespace VkStatusChanger.Worker.Helpers;

internal static class ReflectionHelper
{
    public static void SetPrivatePropertyValue<TValue>(object obj, string propertyName, TValue value)
    {
        Type type = obj.GetType();
        var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property == null)
            throw new ArgumentOutOfRangeException(nameof(propertyName), $"Свойство {propertyName} не было найдено в типе {type.FullName}");
        property.SetMethod.Invoke(obj, new object[] { value! });
    }
}
