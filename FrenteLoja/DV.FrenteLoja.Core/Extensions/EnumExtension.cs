using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DV.FrenteLoja.Core.Extensions
{
	public static class EnumExtension
	{
        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }
    }
}