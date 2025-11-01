using System.ComponentModel;
using System.Reflection;

namespace XADAD7112_Application.Models.System
{
    public class Enums
    {
        public enum UserRole
        {
            [Description("Farmer")]
            Farmer = 0,

            [Description("Employee")]
            Employee = 1,
        }
    }  

    public static class EnumExtensions
        {
            public static string GetEnumDescription(this Enum value)
            {
                var field = value.GetType().GetField(value.ToString());
                if (field == null) return value.ToString();

                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                return attribute?.Description ?? value.ToString();
            }
        }
}
