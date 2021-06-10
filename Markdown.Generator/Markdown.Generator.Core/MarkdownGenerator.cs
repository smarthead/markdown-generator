using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Markdown.Generator.Core
{
    public static class MarkdownGenerator
    {
        public static MarkdownableType[] Load(string dllPath, string namespaceMatch)
        {
            var xmlPath = Path.Combine(Directory.GetParent(dllPath)?.FullName ?? throw new InvalidOperationException(), Path.GetFileNameWithoutExtension(dllPath) + ".xml");

            var comments = Array.Empty<XmlDocumentComment>();
            if (File.Exists(xmlPath))
                comments = VsDocParser.ParseXmlComment(XDocument.Parse(File.ReadAllText(xmlPath)), namespaceMatch);
            
            var commentsLookup = comments.ToLookup(x => x.ClassName);

            var namespaceRegex = 
                !string.IsNullOrEmpty(namespaceMatch) ? new Regex(namespaceMatch) : null;

            var markdownableTypes = new[] { Assembly.LoadFrom(dllPath) }
                .SelectMany(x =>
                {
                    try
                    {
                        return x.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        return ex.Types.Where(t => t != null);
                    }
                    catch
                    {
                        return Type.EmptyTypes;
                    }
                })
                .Where(x => x != null)
                .Where(x => x.IsPublic && !typeof(Delegate).IsAssignableFrom(x) && !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                .Where(x => IsRequiredNamespace(x, namespaceRegex))
                .Select(x => new MarkdownableType(x, commentsLookup))
                .ToArray();


            return markdownableTypes;
        }

        private static bool IsRequiredNamespace(Type type, Regex regex) =>
            regex switch
            {
                null => true,
                _ => regex.IsMatch(type.Namespace ?? string.Empty)
            };
    }
}
