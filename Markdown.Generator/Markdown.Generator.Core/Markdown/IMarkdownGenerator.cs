using System;
using System.Reflection;

namespace Markdown.Generator.Core.Markdown
{
    public interface IMarkdownGenerator
    {
        MarkdownableType[] Load(string dllPath, string namespaceMatch);
        MarkdownableType[] Load(Assembly[] assemblies, string namespaceMatch);
        MarkdownableType[] Load(Type[] types);
    }
}