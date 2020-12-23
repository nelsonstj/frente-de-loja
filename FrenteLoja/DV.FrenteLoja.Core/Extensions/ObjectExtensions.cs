using System;
using System.Reflection;
using System.Text;

namespace DV.FrenteLoja.Core.Extensions
{
	public static class ObjectExtensions
	{
		private static PropertyInfo[] _propertyInfos;

		public static string ToStringLog(this object obj)
		{
			if (_propertyInfos == null)
				_propertyInfos = obj.GetType().GetProperties();

			var sb = new StringBuilder();

			foreach (var info in _propertyInfos)
			{
				var value = info.GetValue(obj, null) ?? "(null)";
				sb.AppendLine(info.Name + ": " + value);
			}

			return sb.ToString();
		}
	}
}