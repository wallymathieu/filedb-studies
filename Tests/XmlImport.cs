using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests;

public class XmlImport
{
    private readonly XNamespace _ns;
    private readonly XDocument _xDocument;
    public XmlImport(XDocument xDocument, XNamespace ns)
    {
        _ns = ns;
        _xDocument = xDocument;
    }
    private TTargetType Parse<TTargetType>(XElement target, string typeName, Action<string, PropertyInfo>? onIgnore)
    {
        var props = typeof(TTargetType).GetProperties();
        var @object = Activator.CreateInstance(typeof(TTargetType));
        foreach (var propertyInfo in props)
        {
            var propElement = target.Element(_ns + propertyInfo.Name);
            if (null == propElement) continue;
            if (!(propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)))
            {
                onIgnore?.Invoke(typeName, propertyInfo);
            }
            else
            {
                var value = Convert.ChangeType(propElement.Value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
                propertyInfo.SetValue(@object, value, null);
            }
        }
        return (TTargetType)@object!;
    }

    public IEnumerable<T> Parse<T>(string type, Action<string, PropertyInfo>? onIgnore = null)
    {
        var db = _xDocument.Root;

        var elements = db!.Elements(_ns + type);

        return elements.Select(element => Parse<T>(element, type, onIgnore)).ToList();
    }
    public IEnumerable<(int, int)> ParseConnections(string name, string first, string second)
    {
        var ns = _ns;
        var db = _xDocument.Root;
        var elements = db!.Elements(ns + name);
        return (
            from element in elements
            let f = element.Element(ns + first)
            let s = element.Element(ns + second)
            let firstValue = int.Parse(f.Value)
            let secondValue = int.Parse(s.Value)
            select (firstValue, secondValue)).ToList();
    }
}