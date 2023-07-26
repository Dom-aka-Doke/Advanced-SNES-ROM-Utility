using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Advanced_SNES_ROM_Utility.Helper
{
    public static partial class SNESROMHelper
    {
        public static string GetEnumDescription(Enum enumValue)
        {
            try
            {
                FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

                DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any())
                {
                    return attributes.First().Description;
                }

                return enumValue.ToString();
            }

            catch
            {
                return string.Empty;
            }
        }
    }
}