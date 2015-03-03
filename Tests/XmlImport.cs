using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests
{
	public class XmlImport
	{
		XNamespace _ns;
		XDocument xDocument;
		public XmlImport(XDocument xDocument, XNamespace ns)
		{
			_ns = ns;
			this.xDocument = xDocument;
		}
		private object Parse(XElement target, Type type, Action<Type, PropertyInfo> onIgnore)
		{
			var props = type.GetProperties();
            var parameters = new Dictionary<string,object>();
			foreach (var propertyInfo in props)
			{
				XElement propElement = target.Element(_ns + propertyInfo.Name);
                if (null != propElement)
                {
                    if (!(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)))
                    {
                        onIgnore(type, propertyInfo);
                        parameters.Add(propertyInfo.Name, GetDefaultValue(propertyInfo.PropertyType));
                    }
                    else
                    {
                        var value = Convert.ChangeType(propElement.Value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
                        //propertyInfo.SetValue(@object, value, null);
                        parameters.Add(propertyInfo.Name, value);
                    }
                }
                else
                {
                    parameters.Add(propertyInfo.Name, GetDefaultValue(propertyInfo.PropertyType));
                }
			}
            var @object = Activator.CreateInstance(type, parameters.Values.ToArray(),new object[0]);

			return @object;
		}

        private static object GetDefaultValue(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition()== typeof(IEnumerable<>))
            {
                var listType = typeof(List<>).MakeGenericType(t.GetGenericArguments().Single());
                return Activator.CreateInstance(listType);
            }

            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }

		public IEnumerable<Tuple<Type, Object>> Parse(IEnumerable<Type> types, Action<Type, Object> onParsedEntity = null, Action<Type, PropertyInfo> onIgnore = null)
		{
			var db = xDocument.Root;
			var list = new List<Tuple<Type, Object>>();

			foreach (var type in types)
			{
				var elements = db.Elements(_ns + type.Name);

				foreach (var element in elements)
				{
					var obj = Parse(element, type, onIgnore);
					if (null != onParsedEntity) onParsedEntity(type, obj);
					list.Add(Tuple.Create(type, obj));
				}
			}
			return list;
		}
		public IEnumerable<Tuple<int, int>> ParseConnections(string name, string first, string second, Action<int, int> onParsedEntity = null)
		{
			var ns = _ns;
			var db = xDocument.Root;
			var elements = db.Elements(ns + name);
			var list = new List<Tuple<int, int>>();
			foreach (var element in elements)
			{
				XElement f = element.Element(ns + first);
				XElement s = element.Element(ns + second);
				var firstValue = int.Parse(f.Value);
				var secondValue = int.Parse(s.Value);
				if (null != onParsedEntity) onParsedEntity(firstValue, secondValue);
				list.Add(Tuple.Create(firstValue, secondValue));
			}
			return list;
		}
	}
}
