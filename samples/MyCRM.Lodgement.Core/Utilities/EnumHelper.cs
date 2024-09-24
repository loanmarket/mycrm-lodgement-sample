using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace MyCRM.Lodgement.Common.Utilities;

public static class EnumHelper
{
    public static T ConvertToEnum<T>(string enumString) where T : Enum
    {
        var enumType = typeof(T);
        var enumFields = enumType.GetFields();
        foreach (var field in enumFields)
        {
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attribute != null && attribute.Value == enumString)
            {
                return (T)field.GetValue(null);
            }

            if (field.Name.Equals(enumString, StringComparison.OrdinalIgnoreCase))
            {
                return (T)field.GetValue(null);
            }
            
        }
        throw new ArgumentException($"Unknown value '{enumString}' for enum {enumType.Name}");
    }
}