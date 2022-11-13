using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests
{
    public class XmlImport
    {
        private readonly XNamespace _ns;
        private readonly XDocument xDocument;
        public XmlImport(XDocument xDocument, XNamespace ns)
        {
            _ns = ns;
            this.xDocument = xDocument;
        }
        private TTargetType Parse<TTargetType>(XElement target, string typeName, Action<string, PropertyInfo> onIgnore)
        {
            var props = typeof(TTargetType).GetProperties();
            var @object = Activator.CreateInstance(typeof(TTargetType));
            foreach (var propertyInfo in props)
            {
                XElement propElement = target.Element(_ns + propertyInfo.Name);
                if (null != propElement)
                {
                    if (!(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)))
                    {
                        if (null != onIgnore)
                            onIgnore(typeName, propertyInfo);
                    }
                    else
                    {
                        var value = Convert.ChangeType(propElement.Value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
                        propertyInfo.SetValue(@object, value, null);
                    }
                }
            }
            return (TTargetType)@object;
        }

        public IEnumerable<T> Parse<T>(string type, Action<string, PropertyInfo> onIgnore = null)
        {
            var db = xDocument.Root;
            var list = new List<T>();

            var elements = db.Elements(_ns + type);
            foreach (var element in elements)
            {
                var obj = Parse<T>(element, type, onIgnore);
                list.Add(obj);
            }
        
            return list;
        }
        public IEnumerable<(int, int)> ParseConnections(string name, string first, string second)
        {
            var ns = _ns;
            var db = xDocument.Root;
            var elements = db.Elements(ns + name);
            var list = new List<(int, int)>();
            foreach (var element in elements)
            {
                XElement f = element.Element(ns + first);
                XElement s = element.Element(ns + second);
                var firstValue = int.Parse(f.Value);
                var secondValue = int.Parse(s.Value);
                list.Add((firstValue, secondValue));
            }
            return list;
        }
    }
}
