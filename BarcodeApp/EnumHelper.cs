using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BarcodeApp
{
	public static class EnumHelper
	{
		public static Dictionary<int, string> EnumToDictionary(Type enumerator)
		{
			var dictionary = new Dictionary<int, string>();

			foreach (int key in Enum.GetValues(enumerator))
			{
				//var description = GetEnumDescription((Enum)Enum.Parse(enumerator, value.ToString()));
				//dictionary.Add(value, description);
				string value = Enum.GetName(enumerator, key);
				dictionary.Add(key, value);
			}

			return dictionary;
		}

		//public static string GetEnumDescription(Enum value)
		//{
		//	FieldInfo fi = value.GetType().GetField(value.ToString());
		//	DescriptionAttribute[] attr = DirectCast(fi.GetCustomAttributes(typeof(DescriptionAttribute), false), typeof(DescriptionAttribute));
		//	//var attr = (IEnumerable<DescriptionAttribute>)fi.GetCustomAttributes(typeof(DescriptionAttribute));
		//	//var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute));
		//	if (attr. > 0)
		//	//	return attr[0].Description;
		//	//else
		//		return value.ToString();
		
		//}

		private static T DirectCast<T>(object o, Type type) where T : class
		{
			// https://stackoverflow.com/questions/2683847/cs-equivalent-to-vb-nets-directcast#2684315
			if (!(type.IsInstanceOfType(o)))
			{
				throw new ArgumentException();
			}
			T value = o as T;
			if (value == null && o != null)
			{
				throw new InvalidCastException();
			}
			return value;
		}

		private static T DirectCast<T>(object o) where T : class
		{
			T value = o as T;
			if (value == null && o != null)
			{
				throw new InvalidCastException();
			}
			return value;
		}
	}
}