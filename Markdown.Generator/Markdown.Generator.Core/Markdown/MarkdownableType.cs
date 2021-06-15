using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Markdown.Generator.Core.Markdown
{
    public class MarkdownableType
    {
        private readonly Type _type;
        private readonly ILookup<string, XmlDocumentComment> _commentLookup;

        public string Namespace => _type.Namespace;
        public string Name => _type.Name;
        public string BeautifyName => Beautifier.BeautifyType(_type);
        
        public MarkdownableType(Type type, ILookup<string, XmlDocumentComment> commentLookup)
        {
            _type = type;
            _commentLookup = commentLookup;
        }

        public MethodInfo[] GetMethods()
        {
            return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any() && !x.IsPrivate)
                .ToArray();
        }

        public PropertyInfo[] GetProperties()
        {
            return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.GetProperty | BindingFlags.SetProperty)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                .Where(y =>
                {
                    var get = y.GetGetMethod(true);
                    var set = y.GetSetMethod(true);
                    if (get != null && set != null)
                    {
                        return !(get.IsPrivate && set.IsPrivate);
                    }
                    else if (get != null)
                    {
                        return !get.IsPrivate;
                    }
                    else if (set != null)
                    {
                        return !set.IsPrivate;
                    }
                    else
                    {
                        return false;
                    }
                })
                .ToArray();
        }

        public FieldInfo[] GetFields()
        {
            return _type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.GetField | BindingFlags.SetField)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any() && !x.IsPrivate)
                .ToArray();
        }

        public EventInfo[] GetEvents()
        {
            return _type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                .ToArray();
        }

        private FieldInfo[] GetStaticFields()
        {
            return _type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.GetField | BindingFlags.SetField)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any() && !x.IsPrivate)
                .ToArray();
        }

        public PropertyInfo[] GetStaticProperties()
        {
            return _type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.GetProperty | BindingFlags.SetProperty)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                .Where(y =>
                {
                    var get = y.GetGetMethod(true);
                    var set = y.GetSetMethod(true);
                    if (get != null && set != null)
                    {
                        return !(get.IsPrivate && set.IsPrivate);
                    }
                    else if (get != null)
                    {
                        return !get.IsPrivate;
                    }
                    else if (set != null)
                    {
                        return !set.IsPrivate;
                    }
                    else
                    {
                        return false;
                    }
                })
                .ToArray();
        }

        public MethodInfo[] GetStaticMethods()
        {
            return _type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any() && !x.IsPrivate)
                .ToArray();
        }

        public EventInfo[] GetStaticEvents()
        {
            return _type.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(x => !x.IsSpecialName && !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                .ToArray();
        }

        private void BuildTable<T>(MarkdownBuilder mb, string label, T[] array, IEnumerable<XmlDocumentComment> docs, Func<T, string> type, Func<T, string> name, Func<T, string> finalName)
        {
            if (array.Any())
            {
                mb.AppendLine(label);
                mb.AppendLine();

                var head = (_type.IsEnum)
                    ? new[] { "Value", "Name", "Summary" }
                    : new[] { "Type", "Name", "Summary" };

                IEnumerable<T> seq = array;
                if (!_type.IsEnum)
                {
                    seq = array.OrderBy(name);
                }

                var data = seq.Select(item2 =>
                {
                    var summary = docs.FirstOrDefault(x => x.MemberName == name(item2) || x.MemberName.StartsWith(name(item2) + "`"))?.Summary ?? "";
                    return new[] { MarkdownBuilder.MarkdownCodeQuote(type(item2)), finalName(item2), summary };
                });

                mb.Table(head, data);
                mb.AppendLine();
            }
        }

        public override string ToString()
        {
            var mb = new MarkdownBuilder();

            mb.HeaderWithCode(2, Beautifier.BeautifyType(_type, false));
            mb.AppendLine();

            var desc = _commentLookup[_type.FullName].FirstOrDefault(x => x.MemberType == MemberType.Type)?.Summary ?? "";
            if (desc != "") {
                mb.AppendLine(desc);
            }
            {
                var sb = new StringBuilder();

                var stat = (_type.IsAbstract && _type.IsSealed) ? "static " : "";
                var abst = (_type.IsAbstract && !_type.IsInterface && !_type.IsSealed) ? "abstract " : "";
                var classOrStructOrEnumOrInterface = _type.IsInterface ? "interface" : _type.IsEnum ? "enum" : _type.IsValueType ? "struct" : "class";

                sb.AppendLine($"public {stat}{abst}{classOrStructOrEnumOrInterface} {Beautifier.BeautifyType(_type, true)}");
                var impl = string.Join(", ", new[] { _type.BaseType }.Concat(_type.GetInterfaces()).Where(x => x != null && x != typeof(object) && x != typeof(ValueType)).Select(x => Beautifier.BeautifyType(x)));
                if (impl != "")
                {
                    sb.AppendLine("    : " + impl);
                }

                mb.Code("csharp", sb.ToString());
            }

            mb.AppendLine();

            if (_type.IsEnum)
            {
                var underlyingEnumType = Enum.GetUnderlyingType(_type);

                var enums = Enum.GetNames(_type)
                    .Select(x => new { Name = x, Value = (Convert.ChangeType(Enum.Parse(_type, x), underlyingEnumType)) })
                    .OrderBy(x => x.Value)
                    .ToArray();

                BuildTable(mb, "Enum", enums, _commentLookup[_type.FullName], x => x.Value.ToString(), x => x.Name, x => x.Name);
            }
            else
            {
                BuildTable(mb, "Fields", GetFields(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.FieldType), x => x.Name, x => x.Name);
                BuildTable(mb, "Properties", GetProperties(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.PropertyType), x => x.Name, x => x.Name);
                BuildTable(mb, "Events", GetEvents(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.EventHandlerType), x => x.Name, x => x.Name);
                BuildTable(mb, "Methods", GetMethods(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.ReturnType), x => x.Name, x => Beautifier.ToMarkdownMethodInfo(x));
                BuildTable(mb, "Static Fields", GetStaticFields(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.FieldType), x => x.Name, x => x.Name);
                BuildTable(mb, "Static Properties", GetStaticProperties(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.PropertyType), x => x.Name, x => x.Name);
                BuildTable(mb, "Static Methods", GetStaticMethods(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.ReturnType), x => x.Name, x => Beautifier.ToMarkdownMethodInfo(x));
                BuildTable(mb, "Static Events", GetStaticEvents(), _commentLookup[_type.FullName], x => Beautifier.BeautifyType(x.EventHandlerType), x => x.Name, x => x.Name);
            }

            return mb.ToString();
        }
    }
}