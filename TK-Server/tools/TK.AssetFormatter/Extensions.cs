using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace tk.assetformatter
{
    public static class Extensions
    {
        public static async Task<XElement[]> FetchNodesAsync(this XElement elements, string node) => await Task.Run(() => elements.GetXmlsByNode(node).ToArray());

        public static T GetAttribute<T>(this XElement e, string n, T def = default)
        {
            if (e.Attribute(n) == null) return def;

            var val = e.Attribute(n).Value;
            var t = typeof(T);

            if (t == typeof(string)) return (T)Convert.ChangeType(val, t);
            else if (t == typeof(ushort)) return (T)Convert.ChangeType(Convert.ToUInt16(val, 16), t);
            else if (t == typeof(int)) return (T)Convert.ChangeType(GetInt(val), t);
            else if (t == typeof(uint)) return (T)Convert.ChangeType(Convert.ToUInt32(val, 16), t);
            else if (t == typeof(double)) return (T)Convert.ChangeType(double.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(float)) return (T)Convert.ChangeType(float.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(bool)) return (T)Convert.ChangeType(string.IsNullOrWhiteSpace(val) || bool.Parse(val), t);

            Console.WriteLine($"Type of {t} is not supported by this method, returning default value: {def}...");

            return def;
        }

        public static int GetInt(string x) => x.Contains("x") ? Convert.ToInt32(x, 16) : int.Parse(x);

        public static T GetValue<T>(this XElement e, string n, T def = default)
        {
            if (e.Element(n) == null)
                return def;

            var val = e.Element(n).Value;
            var t = typeof(T);

            if (t == typeof(string)) return (T)Convert.ChangeType(val, t);
            else if (t == typeof(ushort)) return (T)Convert.ChangeType(Convert.ToUInt16(val, 16), t);
            else if (t == typeof(int)) return (T)Convert.ChangeType(GetInt(val), t);
            else if (t == typeof(uint)) return (T)Convert.ChangeType(Convert.ToUInt32(val, 16), t);
            else if (t == typeof(double)) return (T)Convert.ChangeType(double.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(float)) return (T)Convert.ChangeType(float.Parse(val, CultureInfo.InvariantCulture), t);
            else if (t == typeof(bool)) return (T)Convert.ChangeType(string.IsNullOrWhiteSpace(val) || bool.Parse(val), t);

            Console.WriteLine($"Type of {t} is not supported by this method, returning default value: {def}...");

            return def;
        }

        public static IEnumerable<XElement> GetXmlsByNode(this XElement elements, string node)
        {
            foreach (var element in elements.XPathSelectElements($"//{node}"))
                yield return element;
        }

        public static IEnumerable<XElement> GetXmlsByNode(this XElement elements, string node, string value)
        {
            foreach (var element in elements.XPathSelectElements($"//{node}"))
                if (element.Value.Equals(value))
                    yield return element.Parent;
        }

        public static bool HasAttribute(this XElement e, string name) => e.Attribute(name) != null;

        public static bool HasElement(this XElement e, string name) => e.Element(name) != null;
    }
}
