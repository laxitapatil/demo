using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Core
{
    public static class Utility
    {
        public static string GetEnumDisplayName(Enum value)
        {
            IEnumerable<DisplayAttribute> attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes<DisplayAttribute>();
            return attributes.Count() > 0 ? attributes.First().Name : value.ToString();
        }
    }
}