using System;
using System.Reflection;
using Markdown.Generator.Core.Markdown;

namespace Markdown.Generator.Core.Documents
{
    public interface IDocumentBuilder<T> where T:IMarkdownGenerator
    {
        void Generate(Type[] types, string folder);
        
        void Generate(string dllPath, string namespaceMatch, string folder);
        
        void Generate(Assembly[] assemblies, string namespaceMatch, string folder);
    }
}