using System.ComponentModel;
using System.Reflection;

namespace TNZAPI.NET.Extensions
{
    internal static class EnumExtensions
    {
        internal static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            string name = Enum.GetName(enumType, value);

            if (name != null)
            {
                FieldInfo field = enumType.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }

                    // If [Description] attribute is not defined, fallback to using the enum value
                    return name;
                }
            }

            return value.ToString();
        }
    }
}
